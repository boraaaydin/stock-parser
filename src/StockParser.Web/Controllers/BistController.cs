using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Data.Services;

namespace StockParser.Web.Controllers
{
    public class BistController : Controller
    {
        private StockService _service;

        public BistController(StockService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _service.InsertAllData();
            return Ok(result.Message);
        }


    }
}