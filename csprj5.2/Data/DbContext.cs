using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using csprj5._2.Models;
using csprjclib.Models;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<MonitoredFile> MonitoredFiles { get; set; }

        public DbSet<MonitorFrequency> MonitorFrequencies { get; set; }

        public DbSet<MonitoredFileHash> MonitoredFileHashes { get; set; }

        public DbSet<MonitoredFileContent> MonitoredFileContents { get; set; }

        public DbSet<MonitoredFileProperty> MonitoredFileProperties { get; set; }
    }
