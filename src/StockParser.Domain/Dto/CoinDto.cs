﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Domain.Dto
{
    public class CoinDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
    }
}
