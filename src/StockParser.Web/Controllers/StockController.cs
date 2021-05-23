using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Data;
using StockParser.Data.Services;
using StockParser.NoSql.Models;

namespace StockParser.Web.Controllers
{
    public class StockController : Controller
    {
        private MongoContextService _service;

        public StockController(MongoContextService service)
        {
            _service = service;
        }

        /// <summary>
        /// Insert Bist KeyName pairs to StockContext
        /// </summary>
        /// <returns></returns>
        public async Task Insert()
        {
            var contextGroup = new List<StockGroup>
            {
                await _service.CreateBistKeyGroup()
            };
            await _service.CreateStockGroups(contextGroup);
        }

        [HttpPost]
        public async Task<IEnumerable<String>> GetStockNameValueList(string id)
        {
            try
            {
                var list = new List<String>();
                if (!string.IsNullOrEmpty(id))
                {
                    list = (await _service.GetStockNameValueList(id)).Select(x => x.StockKey).ToList();
                }
                return list;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}