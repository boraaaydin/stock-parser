using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.NoSql.Models
{
    public class StockGroup : BaseMongoModel
    {
        public string GroupName { get; set; }
        public List<StockKeyValue> StockKeyValuelist { get; set; }
    }

    public class StockKeyValue
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }

}
