using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Data;
using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using StockParser.NoSql.Services;

namespace StockParser.Web.Controllers
{
    public class ProfileController : Controller
    {
        private MongoProfileService _service;

        public ProfileController(MongoProfileService service, StockContext stockContext)
        {
            _service = service;
            StockContext = stockContext;
        }
        public Guid userId { get; set; } = new Guid("610b083a-3c17-456b-baca-5b7fde4d88a6");
        public StockContext StockContext { get; }

        public  async Task<IActionResult> Index()
        {
            var profile = await _service.GetProfile(userId);
            var viewmodel = profile.ConvertToDto(await StockContext.GetBist());
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
        public IActionResult InsertRule()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertRule(RuleDto rule)
        {
            var profile = await _service.InsertRule(userId, rule);
            return View();
        }

    }
}