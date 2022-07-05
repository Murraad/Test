using Microsoft.AspNetCore.Mvc;
using Vessels.Repositories;

namespace Vessels.Controllers
{
    public class VesselsController : Controller
    {
        private readonly IDataRepository dataRepository;

        public VesselsController(IDataRepository dataRepository) => this.dataRepository = dataRepository;

        // GET: Home
        public async Task<IActionResult> Index() => View(await this.dataRepository.GetVesselsAsync());
    }
}
