namespace DbInterface.Models
{
    public class FkData
    {
        public string SourceColumn { get; set; }
        public string RefTable { get; set; }
        public string RefColumn { get; set; }
    }
}