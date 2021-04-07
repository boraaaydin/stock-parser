using StockParser.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.NoSql.Models
{
    public class Profile : BaseMongoModel
    {
        public Profile()
        {
            Rules = new List<Rule>();
            Ownings = new List<Owning>();
            Solds = new List<Owning>();
        }
        public Guid UserId { get; set; }
        public List<Rule> Rules { get; set; }
        public List<Owning> Ownings { get; set; }
        public List<Owning> Solds { get; set; }
    }

}
