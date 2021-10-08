using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace csprj5._2.ViewModels
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
        public int DelayId { get; set; }

        [Display(Name = "Delay")]
        public string DelayName { get; set; }

    }
}
