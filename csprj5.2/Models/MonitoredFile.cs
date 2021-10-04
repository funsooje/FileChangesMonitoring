using System;
namespace csprj5._2.Models
{
    public class MonitoredFile
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public MonitorFrequency Delay { get; set; }
    }
}

/* Monitored File will hold the file being monitored
 * Fileds: 
 *      Name of File
 *      Location of File
 *      Delay -> This should expand to another class MonitorFrequency (30 secs, 1m, 2m, 5m, 10m, 20m, 30m, 1h, 2h, 3h, 6h,
 */