using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using csprjclib.ApiModels;
using csprjclib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace csprj5._2.Controllers
{
    public class AgentController : Controller
    {
        private readonly DbContext _context;

        public AgentController(DbContext context)
        {
            _context = context;
        }

        // GET: All monitors
        [HttpGet]
        public List<MonitoredFile> GetAllMonitors()
        {
            return _context.MonitoredFiles
                .Include(x => x.Delay)
                .ToList();
        }

        // GET: All active monitors
        [HttpGet]
        public List<MonitoredFile> GetActiveMonitors()
        {
            return _context.MonitoredFiles
                .Where(x => x.Enabled)
                .Include(x => x.Delay)
                .ToList();
        }

        // POST: Update Hash
        [HttpPost]
        public int HashCheck([FromBody]apiMonitoredFileContent hashed)
        {
            var mf = _context.MonitoredFiles.FirstOrDefault(x => x.Id == hashed.Id);
            if (mf == null)
            {
                return 0; // error
            }
            var cDate = DateTime.Now;

            // Hash is always sent
            // PropHash and PropContent is always sent if enabled
            // Only Content is sent conditionally

            if (hashed.Param == "H+C")
            {
                // save Hash
                _context.Add(new MonitoredFileHash
                {
                    Hash = hashed.Hash,
                    HashDate = cDate,
                    MonitoredFile = mf
                });

                //save file
                _context.Add(new MonitoredFileContent
                {
                    Content = hashed.Content,
                    ContentDate = cDate,
                    MonitoredFile = mf
                });
                _context.SaveChanges();
            }
            else
            {
                // Hash Check

                var lastHash = _context.MonitoredFileHashes.Where(x => x.MonitoredFile.Id == mf.Id).OrderByDescending(x => x.HashDate).FirstOrDefault();

                if (lastHash == null)
                {
                    if (mf.MonitorJustHash)
                    {
                        _context.Add(new MonitoredFileHash
                        {
                            Hash = hashed.Hash,
                            HashDate = cDate,
                            MonitoredFile = mf
                        });
                        _context.SaveChanges();
                    }
                    else
                    {
                        return 2; // ask for contents
                    }
                }
                else
                {
                    // compare hash
                    if (lastHash.Hash != hashed.Hash)
                    {
                        if (!mf.MonitorJustHash)
                        {
                            return 2; // ask for contents and properties
                        }
                    }
                }
            }

            // Property Hash Check
            if (mf.MonitorProperties)
            {
                var lastPropHash = _context.MonitoredFileProperties.Where(x => x.MonitoredFile.Id == mf.Id).OrderByDescending(x => x.HashDate).FirstOrDefault();

                if (lastPropHash == null)
                {
                    _context.Add(new MonitoredFileProperty
                    {
                        Hash = hashed.PropertiesHash,
                        HashDate = cDate,
                        MonitoredFile = mf,
                        Property = hashed.Properties
                    });
                    _context.SaveChanges();
                }
                else
                {
                    // compare hash
                    if (lastPropHash.Hash != hashed.PropertiesHash)
                    {
                        _context.Add(new MonitoredFileProperty
                        {
                            Hash = hashed.PropertiesHash,
                            HashDate = cDate,
                            MonitoredFile = mf,
                            Property = hashed.Properties
                        });
                        _context.SaveChanges();
                    }
                }
            }

            return 1; // do nothing
        }

    }
}
