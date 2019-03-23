using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stocker.Data
{
    public class WebParser
    {

        public async Task GetData()
        {
            var mainUrl = "http://push.bigpara.com/borsa/hisse-fiyatlari/";
            var harflist = new List<string> { "A", "B", "C", "D","E","F","G","H","I","J","K","L","M","N","O","P","R","S","T","U","V","Y","Z" };
            var taskList = new List<Task<List<StockDto>>>();
            foreach(var harf in harflist)
            {
                var url = new Uri(new Uri(mainUrl), $"{harf}-harfi-ile-baslayan-hisseler");
                taskList.Add(GetDataPerPage(url));
            }

            await Task.WhenAll(taskList);

            var bulkStockList = new List<StockDto>();
            var index = 0;
            
            foreach(var task in taskList)
            {
                var stocks=await task;
                bulkStockList.AddRange(stocks);
                Console.WriteLine( $"{harflist[index]} ile başlayan {stocks.Count} hisse eklendi");
                index++;
            }
            Console.WriteLine($"Toplam {bulkStockList.Count} hisse çekildi");
        }

        public async Task<List<StockDto>> GetDataPerPage(Uri url)
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
                        stock.Title = aTag.InnerHtml;
                    }
                    list.Add(coloum.InnerHtml);

                }
                    var numbers=new List<decimal>();
                foreach(var a in list)
                {
                    var b = a.Replace(@"\r\n", "").Trim();
                    if(Decimal.TryParse(b, out var dec))
                    {
                        numbers.Add(dec);
                    }
                }
                stock.Final = numbers[0];
                stock.Yesterday= numbers[1];
                stock.DailyChange= numbers[2];
                stock.Highest= numbers[3];
                stock.Lowest= numbers[4];
                stock.Average= numbers[5];
                stock.VolumeLot= numbers[6];
                stock.VolumeTL= numbers[7];
                stockList.Add(stock);
            }

            return stockList;
        }
    }

    public class StockDto
    {
        public string Title { get; set; }
        public decimal Final { get; set; }
        public decimal Yesterday { get; set; }
        public decimal DailyChange { get; set; }
        public decimal Highest { get; set; }
        public decimal Lowest { get; set; }
        public decimal Average { get; set; }
        public decimal VolumeLot { get; set; }
        public decimal VolumeTL { get; set; }
    }
}
