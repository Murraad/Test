﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vessels.Data.Models;
using Vessels.Repositories;

namespace Vessels.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataRepository dataRepository;

        public HomeController(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
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
        public async Task<IActionResult> Create([Bind("Id,Date,Latitude,Longitude,VesselIMO")] VesselPosition vesselPosition)
        {
            if (ModelState.IsValid) 
            {
                await this.dataRepository.AddPositionAsync(vesselPosition);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Latitude,Longitude,VesselIMO")] VesselPosition vesselPosition)
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
    }
}