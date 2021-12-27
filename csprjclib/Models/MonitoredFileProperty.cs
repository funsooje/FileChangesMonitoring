using System;
using System.ComponentModel.DataAnnotations;

namespace csprjclib.Models
{
    public class MonitoredFileProperty
    {
        [Key]
        public int Id { get; set; }

        public MonitoredFile MonitoredFile { get; set; }

        public string Property { get; set; }

        public string Hash { get; set; }

        public DateTime HashDate { get; set; }
    }
}
