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

        // POST: Update Hash
        [HttpPost]
        public int HashCheck([FromBody]apiMonitoredFileHash hashed)
        {
            var mf = _context.MonitoredFiles.FirstOrDefault(x => x.Id == hashed.Id);
            if (mf == null)
            {
                return 0; // error
            }
            var lastHash = _context.MonitoredFileHashes.Where(x => x.MonitoredFile.Id == mf.Id).OrderByDescending(x => x.HashDate).FirstOrDefault();
            if (lastHash == null)
            {
                _context.Add(new MonitoredFileHash
                {
                    Hash = hashed.Hash,
                    HashDate = DateTime.Now,
                    MonitoredFile = mf
                });
                _context.SaveChanges();
                return 2; // provide file contents
            }
            else
            {
                // compare hash
                if (lastHash.Hash == hashed.Hash)
                {
                    // do nothing
                    return 1;
                }
                else
                {
                    // create new hash entry
                    _context.Add(new MonitoredFileHash
                    {
                        Hash = hashed.Hash,
                        HashDate = DateTime.Now,
                        MonitoredFile = mf
                    });
                    _context.SaveChanges();
                    return 2; // provide file contents
                }
            }


        }

        //POST: Update file
        [HttpPost]
        public int UploadFile([FromBody]apiMonitoredFileContent model)
        {
            var mf = _context.MonitoredFiles.FirstOrDefault(x => x.Id == model.Id);
            if (mf == null)
            {
                return 0; // error
            }

            //save file
            _context.Add(new MonitoredFileContent
            {
                Content = model.Content,
                ContentDate = DateTime.Now,
                MonitoredFile = mf
            });
            _context.SaveChanges();
            return 1;

        }
    }
}
