using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockParser.Common;
using StockParser.Data;

namespace StockParser.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private ParserService _parserService;

        public StockController(ParserService parserService)
        {
            _parserService = parserService;
        }

        [HttpGet("InsertStock")]
        public async Task<IActionResult> InsertStock()
        {
            var result = await _parserService.InsertStockData();
            if (result.Status == Common.ServiceStatus.Ok)
            {
                return Ok(new ApiResponseSuccess(result.Message));
            }
            return Ok(new ApiResponseError(result.Message));
        }
    }
}