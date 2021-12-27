using System;
namespace csprjclib.ApiModels
{
    public class apiMonitoredFileContent
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public string Hash { get; set; }

        public string Properties { get; set; }

        public string PropertiesHash { get; set; }

        public string Param { get; set; } // H - Hash; H+C - Hash, content and properties

        
    }
}
