using System;
using System.ComponentModel.DataAnnotations;

namespace csprjclib.Models
{
    public class MonitoredFile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter file location")]
        [Display(Name = "File Path", Description = "File path including file name")]
        public string Location { get; set; }

        public MonitorFrequency Delay { get; set; }

        public bool Enabled { get; set; }
    }
}
