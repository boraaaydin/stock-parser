using Microsoft.AspNetCore.Mvc;
using StockParser.Data.CoinMarketApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Web.Controllers
{
    public class RunnerController : Controller
    {
        private CoinMarketService _coinMarketService;

        public RunnerController(CoinMarketService coinMarketService)
        {
            _coinMarketService = coinMarketService;
        }
        public async Task<IActionResult> CoinMarket()
        {
            var data = await _coinMarketService.GetMarket();
            return Ok(data);
        }
    }
}
