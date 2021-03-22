using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using StockParser.NoSql.Services;

namespace StockParser.Web.Controllers
{
    public class ProfileController : Controller
    {
        private MongoProfileService _service;

        public ProfileController(MongoProfileService service)
        {
            _service = service;
        }
        public Guid userId { get; set; } = new Guid("610b083a-3c17-456b-baca-5b7fde4d88a6");
        public  async Task<IActionResult> Index()
        {
            var profile = await _service.GetProfile(userId);
            var viewmodel = profile.ConvertToDto();
            return View(viewmodel);
        }

        public IActionResult InsertOwning()
        {
            var dto = new OwningDto { PurchaseDate = DateTime.UtcNow.Date };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOwning(OwningDto owning)
        {
            var profile = await _service.InsertOwning(userId,owning);
            return View();
        }


    }
}