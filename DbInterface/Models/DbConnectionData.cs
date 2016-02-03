namespace DbInterface.Models
{
    public class DbConnectionData
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string Instance { get; set; }

        public string SeverName { get; set; }
        public string Port { get; set; }

        public string User { get; set; }
        public string Password { get; set; }
    }
}
