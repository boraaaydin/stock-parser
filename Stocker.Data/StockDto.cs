﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Stocker.Data
{
    public class StockDto
    {
        public string StockName { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal YesterdayPrice { get; set; }
        public decimal DailyChange { get; set; }
        public decimal HighestPrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal VolumeLot { get; set; }
        public decimal VolumeTL { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}