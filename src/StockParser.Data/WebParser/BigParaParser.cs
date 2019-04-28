using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using StockParser.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockParser.Data.WebParser
{
    public class BigParaParser:IWebParser
    {
        private ILogger<BigParaParser> _logger;
        private HashSet<StockDto> StockData;

        public BigParaParser(ILogger<BigParaParser> logger)
        {
            _logger = logger;
        }
        public async Task<HashSet<StockDto>> GetStockData()
        {
            if (StockData == null)
            {
                var mainUrl = "http://push.bigpara.com/borsa/hisse-fiyatlari/";
                var letterlist = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "Y", "Z" };
                var taskList = new List<Task<HashSet<StockDto>>>();
                foreach (var letter in letterlist)
                {
                    var url = new Uri(new Uri(mainUrl), $"{letter}-harfi-ile-baslayan-hisseler");
                    taskList.Add(GetDataPerPage(url));
                }

                await Task.WhenAll(taskList);

                StockData = new HashSet<StockDto>();
                var index = 0;

                foreach (var task in taskList)
                {
                    var stocks = await task;
                    StockData.UnionWith(stocks);
                    _logger.LogTrace($"{stocks.Count} stocks parsed starting with {letterlist[index]} ");
                    index++;
                }
                _logger.LogTrace($"Total {StockData.Count} stocks parsed");
            }
            return StockData;
        }

        public async Task<HashSet<StockDto>> GetDataPerPage(Uri url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();

            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var tableDiv = pageDocument.DocumentNode.SelectNodes("//div[@class='tableCnt']//div[@class='tBody']//ul");

            var stockList = new List<StockDto>();
            foreach(var row in tableDiv)
            {
                var stock = new StockDto();
                var list = new List<string>();
                var coloums = row.SelectNodes("li");
                foreach (var coloum in coloums)
                {
                    var aTag = coloum.SelectSingleNode("a");
                    if (aTag != null) {
                        stock.StockName = aTag.InnerHtml;
                    }
                    list.Add(coloum.InnerHtml);

                }
                var numbers=new List<decimal>();
                foreach(var a in list)
                {
                    if(Decimal.TryParse(a.Replace(@"\r\n", "").Trim(), out var dec))
                    {
                        numbers.Add(dec);
                    }
                }
                stock.FinalPrice = numbers[0];
                stock.YesterdayPrice= numbers[1];
                stock.DailyChange= numbers[2];
                stock.HighestPrice= numbers[3];
                stock.LowestPrice= numbers[4];
                stock.AveragePrice= numbers[5];

                if (long.TryParse(list[7].Replace(".",""), out long lot))
                {
                    stock.VolumeLot = lot;
                }
                if (long.TryParse(list[8].Replace(".", ""), out long tl))
                {
                    stock.VolumeTL = tl;
                }
                stockList.Add(stock);
            }

            return new HashSet<StockDto>(stockList);
        }
    }

  
}
