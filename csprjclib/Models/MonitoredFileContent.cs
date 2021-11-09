using System;
using System.ComponentModel.DataAnnotations;

namespace csprjclib.Models
{
    public class MonitoredFileContent
    {
        [Key]
        public int Id { get; set; }

        public MonitoredFile MonitoredFile { get; set; }

        public byte[] Hash { get; set; }

        public DateTime ContentDate { get; set; }
    }
}
