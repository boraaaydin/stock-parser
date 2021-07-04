using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Data;
using StockParser.Data.Services;
using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace StockParser.Web.Controllers
{
    public class ProfileController : Controller
    {
        private ProfileService _service;

        public ProfileController(ProfileService service, ContextService stockContext)
        {
            _service = service;
            StockContext = stockContext;
        }
        public Guid userId { get; set; } = new Guid("610b083a-3c17-456b-baca-5b7fde4d88a6");
        public ContextService StockContext { get; }

        public async Task<IActionResult> Index()
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

        public async Task<IActionResult> SellOwning([FromQuery]string name, [FromQuery] int quantity)
        {
            var stock = await StockContext.GetByName(name);
            var dto = new OwningDto
            {
                PurchaseQuantity = quantity,
                Name = name,
                SellDate = DateTime.UtcNow,
                CurrentValue=stock?.FinalPrice
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> SellOwning(OwningDto owning)
        {
            await _service.SellOwning(userId, owning);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertOwning(OwningDto owning)
        {
            var profile = await _service.InsertOwning(userId, owning);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RuleAddOrUpdate(RuleDto rule)
        {
            var profile = await _service.AddOrUpdateRule(userId, rule);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RuleAddOrUpdate(string id)
        {
            var ruleDto = new RuleDto();
            if (!string.IsNullOrEmpty(id))
            {
                var rule = await _service.GetRule(userId, id);
                var bistDto = (await StockContext.GetBist()).Where(x => x.StockName == id).FirstOrDefault();
                ruleDto = rule.ConvertToDto(bistDto.ConvertToDto());
            }
            return View(ruleDto);
        }

    }
}