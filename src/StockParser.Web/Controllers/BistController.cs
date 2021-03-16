using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Data;

namespace StockParser.Web.Controllers
{
    public class BistController : Controller
    {
        private ParserService _service;

        public BistController(ParserService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var result=await _service.InsertStockData();
            return Ok(result.Message);
        }

 
    }
}