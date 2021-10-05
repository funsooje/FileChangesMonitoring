using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csprj5._2.Models
{
    public class MonitorFrequency
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter frequency name")]
        public string Name { get; set; }

        [Display(Name = "Delay (in secs)")]
        [Required(ErrorMessage ="Please enter monitor delay (in secs)")]
        public int DelayInSecs { get; set; }

        [NotMapped]
        public List<MonitoredFile> MonitoredFiles { get; set; }
    }
}
