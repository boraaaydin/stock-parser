using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using StockParser.Common;
using StockParser.Domain;
using StockParser.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockParser.Data.WebParser
{
    public class BigParaParser:IWebParser
    {
        public BigParaParser(
            //ICustomLogger logger
            )
        {
            //_logger = logger;
        }
        private HashSet<IBistStock> StockData;

        //public ICustomLogger _logger { get; }

        public async Task<ServiceResult<HashSet<IBistStock>>> GetStockData()
        {
            try
            {
                //_logger.LogInformation("GetStockData function called");
                if (StockData == null)
                {
                    StockData = new HashSet<IBistStock>();
                    var mainUrl = "http://push.bigpara.com/borsa/hisse-fiyatlari/";
                    var letterlist = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "Y", "Z" };
                    var taskList = new List<Task<HashSet<IBistStock>>>();
                    foreach (var letter in letterlist)
                    {
                        var url = new Uri(new Uri(mainUrl), $"{letter}-harfi-ile-baslayan-hisseler");
                        taskList.Add(GetDataPerPage(url));
                    }

                    await Task.WhenAll(taskList);

                    //_logger.LogInformation("All pages parsed");
                    foreach (var task in taskList)
                    {
                        var stocks = await task;
                        if (stocks != null && stocks.Count != 0)
                        {
                            StockData.UnionWith(stocks);
                        }
                        //_logger.LogTrace($"{stocks.Count} stocks parsed starting with {letterlist[index]} ");
                    }
                    //_logger.LogTrace($"Total {StockData.Count} stocks parsed");
                }
                return new ServiceResult<HashSet<IBistStock>>(ServiceStatus.Ok, StockData);
            }
            catch (Exception ex)
            {
                //_logger.LogError("GetStockData:"+ex.Message);
                return new ServiceResult<HashSet<IBistStock>>(ServiceStatus.Error, null,"GetStockData hata: "+ex.Message);
            }

        }

        public async Task<HashSet<IBistStock>> GetDataPerPage(Uri url)
        {
            try
            {
                //_logger.LogInformation("GetDataPerPage function called : " + url.ToString());
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                var pageContents = await response.Content.ReadAsStringAsync();

                HtmlDocument pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                var tableDiv = pageDocument.DocumentNode.SelectNodes("//div[@class='tableCnt']//div[@class='tBody']//ul");

                var stockList = new List<IBistStock>();
                foreach (var row in tableDiv)
                {
                    var stock = new IBistStock();
                    var list = new List<string>();
                    var coloums = row.SelectNodes("li");
                    foreach (var coloum in coloums)
                    {
                        var aTag = coloum.SelectSingleNode("a");
                        if (aTag != null)
                        {
                            stock.StockName = aTag.InnerHtml;
                        }
                        //_logger.LogInformation("url:"+ url+". InnerHtml:"+coloum.InnerHtml);
                        list.Add(coloum.InnerHtml);
                    }
                    var numbers = new List<decimal>();
                    foreach (var a in list)
                    {
                        if (Decimal.TryParse(a.Replace(@"\r\n", "").Trim(), out var dec))
                        {
                            numbers.Add(dec);
                        }
                    }
                    stock.FinalPrice = numbers[0];
                    stock.YesterdayPrice = numbers[1];
                    stock.DailyChange = numbers[2];
                    stock.HighestPrice = numbers[3];
                    stock.LowestPrice = numbers[4];
                    stock.AveragePrice = numbers[5];
                    stock.Date = DateTime.UtcNow.Date;

                    if (long.TryParse(list[7].Replace(".", ""), out long lot))
                    {
                        stock.VolumeLot = lot;
                    }
                    if (long.TryParse(list[8].Replace(".", ""), out long tl))
                    {
                        stock.VolumeTL = tl;
                    }
                    stockList.Add(stock);
                }
                return new HashSet<IBistStock>(stockList);
            }
            catch (Exception ex)
            {
                //_logger.LogError("Exception:"+ex.Message);
                return null;
            }
           
        }
    }

  
}
