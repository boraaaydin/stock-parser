using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using StockParser.NoSql.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StockParser.Data.Test
{
    public class CalculatorData
    {
        /// <summary>
        /// 1. deðiþken: sahip olunan miktarlara ait integer listesi
        /// 2. deðiþken: satýþ yapýlan miktar
        /// 3. deðiþken: kalan miktar
        /// </summary>
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {new List<int> {2,3},1, 4},
                new object[] {new List<int> {2,3,6,2},4,9},
                new object[] {new List<int> {2,3,1},6, 0},
                new object[] {new List<int> {8,3,1},3,9},
                new object[] {new List<int> {8,3,1},10,2},
                new object[] {new List<int> {8,3,1},1,11},
                new object[] {new List<int> {1,1,1,1},3,1},
                new object[] {new List<int> {1,1,1,1},4,0},
                new object[] {new List<int> {1,1,1,1},3,1},
            };
    }

    public class SellFromOwning
    {
        const string StockName = "ISCTR";
        Profile profileWith_8_2 = new Profile()
        {
            Solds = new List<Owning>(),
            Ownings = new List<Owning>
                {
                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(1,1,1),
                        PurchaseQuantity=8,
                        PurchaseValue=40
                    },

                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(2,1,1),
                        PurchaseQuantity=2,
                        PurchaseValue=30
                    }
                }
        };
        Profile profileWith_2_3_1 = new Profile()
        {
            Solds = new List<Owning>(),
            Ownings = new List<Owning>
                {
                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(1,1,1),
                        PurchaseQuantity=2,
                        PurchaseValue=40
                    },
                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(2,1,1),
                        PurchaseQuantity=3,
                        PurchaseValue=30
                    },
                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(10,1,1),
                        PurchaseQuantity=1,
                        PurchaseValue=32
                    }
                }
        };
        Profile profileWith_10_5_8_2_10 = new Profile()
        {
            Solds = new List<Owning>(),
            Ownings = new List<Owning>
                {
                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(1,1,1),
                        PurchaseQuantity=10,
                        PurchaseValue=40
                    },
                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(2,1,1),
                        PurchaseQuantity=5,
                        PurchaseValue=30
                    },
                    new Owning{
                        Name=StockName,
                        PurchaseDate=DateTime.UtcNow-new TimeSpan(10,1,1),
                        PurchaseQuantity=8,
                        PurchaseValue=32
                    }
                }
        };

        [Theory]
        [MemberData(nameof(CalculatorData.Data), MemberType = typeof(CalculatorData))]
        public void Should_Match_Remaining_Quantity( List<int> ownings, int soldQuantity, int expected)
        {
            var profile = new Profile();
            foreach(var owning in ownings)
            {
                profile.Ownings.Add(new Owning
                {
                    Name = StockName,
                    PurchaseQuantity = owning,
                    PurchaseDate = DateTime.UtcNow + new TimeSpan(owning, 0, 0, 0),
                    PurchaseValue = 10 + owning
                }) ; 
            }
            var service = new MongoProfileService(null);
            OwningDto soldOwning = new OwningDto()
            {
                Name = StockName,
                PurchaseQuantity = soldQuantity,
                SellValue = 45,
                SellDate = DateTime.UtcNow
            };
            service.SellFromOwning(profile, soldOwning);
            Assert.Equal(expected, profile.Ownings.Where(x => x.Name == soldOwning.Name).Sum(y => y.PurchaseQuantity));
        }


        [Fact]
        public void NotAllSold_Sold_Count_Should_be_2()
        {
            int soldQuantity = 7;
            var profile = profileWith_8_2;
            var service = new MongoProfileService(null);
            OwningDto soldOwning = new OwningDto()
            {
                Name = StockName,
                PurchaseQuantity = soldQuantity,
                SellValue = 45,
                SellDate = DateTime.UtcNow
            };
            service.SellFromOwning(profile, soldOwning);
            Assert.Single(profile.Solds);
        }


        [Fact]
        public void AllSold_Solds_Count_Should_Be_2()
        {
            int soldQuantity = 10;
            var profile = profileWith_8_2;
            var service = new MongoProfileService(null);
            OwningDto soldOwning = new OwningDto()
            {
                Name = StockName,
                PurchaseQuantity = soldQuantity,
                SellValue = 45,
                SellDate = DateTime.UtcNow
            };
            service.SellFromOwning(profile, soldOwning);
            Assert.Equal(2, profile.Solds.Count);
        }

        [Fact]
        public void Should_throw_error_above_quantity()
        {
            int soldQuantity = 12;
            var profile = profileWith_8_2;
            var service = new MongoProfileService(null);
            OwningDto soldOwning = new OwningDto()
            {
                Name = StockName,
                PurchaseQuantity = soldQuantity,
                SellValue = 45,
                SellDate = DateTime.UtcNow
            };
            Assert.Throws<Exception>(()=> service.SellFromOwning(profile, soldOwning));
        }
    }
}
