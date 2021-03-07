using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Data;
using StockParser.Data.WebParser;
using StockParser.Domain;
using StockParser.Domain.Services;

namespace StockParser.Web.Controllers
{
    public class StockController : Controller
    {
        private ParserService _service;

        public StockController(ParserService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var result=await _service.InsertStockData();
            return Ok(result.Message);
        }

        //public async Task<IActionResult> Get()
        //{
        //    try
        //    {
        //        var stocks = await _parser.GetStockData();
        //        var recordOftoday = await _service.GetStock(DateTime.Now.Date, "ISCTR");

        //        if (stocks.Status == ServiceStatus.Ok)
        //        {
        //            return Ok(stocks.Entity);
        //        }
        //        return Ok(stocks.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }

        //}
    }
}