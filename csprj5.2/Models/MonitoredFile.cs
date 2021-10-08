﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace csprj5._2.Models
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
    }
}

/* Monitored File will hold the file being monitored
 * Fileds: 
 *      Name of File
 *      Location of File
 *      Delay -> This should expand to another class MonitorFrequency (30 secs, 1m, 2m, 5m, 10m, 20m, 30m, 1h, 2h, 3h, 6h,
 */