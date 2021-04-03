﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockParser.Data;
using StockParser.Domain.Dto;
using StockParser.NoSql.Services;

namespace StockParser.Web.Controllers
{
    public class StockController : Controller
    {
        private StockContext _context;
        private MongoContextService _service;

        public StockController(StockContext context, MongoContextService service )
        {
            _context = context;
            _service = service;
        }
 
        public async Task Insert()
        {
            await _context.InsertContextKeyNameValues();
        }

        [HttpPost]
        public async Task<IEnumerable<String>> GetStockNameValueList(string id)
        {
            try
            {
                var list = new List<String>();
                if (!string.IsNullOrEmpty(id))
                {
                    list = (await _service.GetStockNameValueList(id)).Select(x=>x.StockKey).ToList();
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