using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Vessels.Data.Models;
using Vessels.Hubs;
using Vessels.Repositories;

namespace Vessels.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataRepository dataRepository;
        private readonly IHubContext<VesselPositionsHub> hubContext;

        public HomeController(IDataRepository dataRepository, IHubContext<VesselPositionsHub> hubContext)
        {
            this.dataRepository = dataRepository;
            this.hubContext = hubContext;
        }

        // GET: Home
        public async Task<IActionResult> Index() => View(await this.dataRepository.GetAllPositionsAsync());

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) { return NotFound(); }

            var vesselPosition = await this.dataRepository.GetPositionAsync((int)id);
            if (vesselPosition is null) { return NotFound(); }

            return View(vesselPosition);
        }

        // GET: Home/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.VesselIMOs = await this.GetVessels();
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(nameof(VesselPosition.Id), nameof(VesselPosition.Date),
            nameof(VesselPosition.Latitude),nameof(VesselPosition.Longitude),nameof(VesselPosition.VesselIMO))] VesselPosition vesselPosition)
        {
            if (ModelState.IsValid) 
            {
                await this.dataRepository.AddPositionAsync(vesselPosition);
                var createdPosition = await dataRepository.GetPositionAsync(vesselPosition.Id);
                if(createdPosition is not null)
                {
                    createdPosition.Vessel.VesselPositions = null;
                    await this.hubContext.Clients.All.SendAsync("ReceiveMessage", createdPosition);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.VesselIMOs = await this.GetVessels();
            return View(vesselPosition);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) { return NotFound(); }

            var vesselPosition = await this.dataRepository.GetPositionAsync((int)id);
            if (vesselPosition is null) { return NotFound(); }
            ViewBag.VesselIMOs = await this.GetVessels();
            return View(vesselPosition);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(nameof(VesselPosition.Id), nameof(VesselPosition.Date),
            nameof(VesselPosition.Latitude),nameof(VesselPosition.Longitude),nameof(VesselPosition.VesselIMO))] VesselPosition vesselPosition)
        {
            if (id != vesselPosition.Id) { return NotFound(); }

            if (ModelState.IsValid)
            {
                try { await this.dataRepository.UpdatePositionAsync(vesselPosition); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VesselPositionExists(vesselPosition.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.VesselIMOs = await this.GetVessels();
            return View(vesselPosition);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) { return NotFound(); }

            var vesselPosition = await this.dataRepository.GetPositionAsync((int)id);
            if (vesselPosition is null) { return NotFound(); }

            return View(vesselPosition);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vesselPosition = await this.dataRepository.GetPositionAsync((int)id);
            if (vesselPosition is not null) { await this.dataRepository.DeletePositionAsync(id); }
            
            return RedirectToAction(nameof(Index));
        }

        private bool VesselPositionExists(int id) => this.dataRepository.PositionExists(id);

        private async Task<SelectList> GetVessels() => new SelectList(await this.dataRepository.GetVesselsAsync(), nameof(Vessel.IMO), nameof(Vessel.Name));

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.dataRepository.Dispose();
        }
    }
}
