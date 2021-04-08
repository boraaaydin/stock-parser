using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockParser.Data;
using StockParser.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CollectApiService _collectApiService;

        public HomeController(ILogger<HomeController> logger, CollectApiService collectApiService)
        {
            _logger = logger;
            _collectApiService = collectApiService;
        }

        public async Task<IActionResult> Index()
        {
             //await _collectApiService.GetCurrency();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
