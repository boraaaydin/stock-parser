using System.Collections.Generic;

namespace StockParser.NoSql.Models
{
    public class KeyNameGroup : BaseMongoModel
    {
        public string GroupName { get; set; }
        public List<KeyNameItem> KeyNameList { get; set; }
    }

    public class KeyNameItem
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }

}
