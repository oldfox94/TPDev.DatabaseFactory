namespace DbInterface.Models
{
    public class ColumnData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DefaultValue { get; set; }

        public bool existsInDB { get; set; }
        public bool isNew { get; set; }
    }
}
