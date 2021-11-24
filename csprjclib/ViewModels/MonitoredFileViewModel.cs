using System;
using System.ComponentModel.DataAnnotations;

namespace csprjclib.ViewModels
{
    public class MonitoredFileViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter file location")]
        [Display(Name = "File Path", Description = "File path including file name")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Please select monitor delay")]
        [Display(Name = "Delay")]
        public int DelayId { get; set; }

        [Display(Name = "Delay")]
        public string DelayName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Content Date")]
        public DateTime ContentDate { get; set; }

        public string Hash { get; set; }

        [Display(Name = "Hash Date")]
        public DateTime HashDate { get; set; }

        public string Enabled { get; set; }
    }
}
