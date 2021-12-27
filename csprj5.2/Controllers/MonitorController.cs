using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using csprj5._2.Models;
using csprjclib.ViewModels;
using csprjclib.Models;
using System.IO;


namespace csprj5._2.Controllers
{
    public class MonitorController : Controller
    {
        private readonly DbContext _context;

        public MonitorController(DbContext context)
        {
            _context = context;
        }

        // GET: Monitor
        public async Task<IActionResult> Index()
        {
            List<MonitoredFileViewModel> viewModels = new List<MonitoredFileViewModel>();

            var mFs = await _context.MonitoredFiles.Include(x => x.Delay).ToListAsync();
            foreach (var item in mFs)
            {
                viewModels.Add(new MonitoredFileViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    DelayName = item.Delay.Name,
                    Location = item.Location,
                    Enabled = item.Enabled ? "Yes" : "No",
                    MonitorJustHash = item.MonitorJustHash ? "Yes" : "No",
                    MonitorProperties = item.MonitorProperties ? "Yes" : "No"
                });
            }
            return View(viewModels);
        }

        // GET: Monitor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitoredFile = await _context.MonitoredFiles
                .Include(x => x.Delay)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitoredFile == null)
            {
                return NotFound();
            }

            var Hashd = _context.MonitoredFileHashes.OrderByDescending(x => x.HashDate).FirstOrDefault(x => x.MonitoredFile.Id == id);
            var Content = _context.MonitoredFileContents.OrderByDescending(x => x.ContentDate).FirstOrDefault(x => x.MonitoredFile.Id == id);

            var viewMF = new MonitoredFileViewModel
            {
                Name = monitoredFile.Name,
                Location = monitoredFile.Location,
                DelayId = monitoredFile.Delay.Id,
                DelayName = monitoredFile.Delay.Name,
                Id = monitoredFile.Id,
                Enabled = monitoredFile.Enabled ? "Yes" : "No",
                MonitorJustHash = monitoredFile.MonitorJustHash ? "Yes" : "No",
                MonitorProperties = monitoredFile.MonitorProperties ? "Yes" : "No"
            };
            if (Hashd != null)
            {
                viewMF.Hash = Hashd.Hash;
                viewMF.HashDate = Hashd.HashDate;
            }
            if (Content != null)
            {
                //byte[] text = Content.Content;               
                viewMF.Content = System.Text.Encoding.Default.GetString(Content.Content); //Convert.ToBase64String(Content.Content);
                viewMF.ContentDate = Content.ContentDate;
                //var tt = System.Text.Encoding.Default.GetString(text);
                //System.IO.File.WriteAllText("/tmp/test", tt);
            }

            return View(viewMF);
        }

        // GET: Monitor/Create
        public IActionResult Create()
        {
            //var model = new MonitoredFile();
            //model.MonitorFrequencies =  new SelectList(_context.MonitorFrequencies.ToList(), "Id", "Name").ToList();
            var freqs = new SelectList(_context.MonitorFrequencies.ToList(), "Id", "Name").ToList();
            ViewBag.MonitorFrequencies = freqs;
            return View();
        }

        // POST: Monitor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonitoredFileViewModel monitoredFile)
        {
            var delay = await _context.MonitorFrequencies.FindAsync(monitoredFile.DelayId);
            if (delay == null)
            {
                return NotFound(); //very unlikely
            }

            if (ModelState.IsValid)
            {
                _context.Add(new MonitoredFile {
                            Delay = delay,
                            Location = monitoredFile.Location,
                            Name = monitoredFile.Name,
                            Enabled = monitoredFile.Enabled == "Yes",
                            MonitorJustHash = monitoredFile.MonitorJustHash == "Yes",
                            MonitorProperties = monitoredFile.MonitorProperties == "Yes"
                        });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monitoredFile);
        }

        // GET: Monitor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ;
            var monitoredFile = await _context.MonitoredFiles.Include(x => x.Delay).FirstOrDefaultAsync(x => x.Id == id);
            if (monitoredFile == null)
            {
                return NotFound();
            }

            var monitoredFileView = new MonitoredFileViewModel
            {
                Name = monitoredFile.Name,
                Id = monitoredFile.Id,
                DelayId = monitoredFile.Delay.Id,
                Location = monitoredFile.Location,
                Enabled = monitoredFile.Enabled ? "Yes" : "No",
                MonitorJustHash = monitoredFile.MonitorJustHash ? "Yes" : "No",
                MonitorProperties = monitoredFile.MonitorProperties ? "Yes" : "No"
            };

            var freqs = new SelectList(_context.MonitorFrequencies.ToList(), "Id", "Name").ToList();
            ViewBag.MonitorFrequencies = freqs;
            return View(monitoredFileView);
        }

        // POST: Monitor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonitoredFileViewModel monitoredFile)
        {
            if (id != monitoredFile.Id)
            {
                return NotFound();
            }

            var delay = await _context.MonitorFrequencies.FindAsync(monitoredFile.DelayId);
            if (delay == null)
            {
                return NotFound(); //very unlikely
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbMF = await _context.MonitoredFiles.Include(x => x.Delay).FirstOrDefaultAsync(x => x.Id == monitoredFile.Id);
                    if (dbMF == null)
                    {
                        return NotFound();
                    }
                    dbMF.Delay = delay;
                    dbMF.Name = monitoredFile.Name;
                    dbMF.Location = monitoredFile.Location;
                    dbMF.Enabled = monitoredFile.Enabled == "Yes";
                    dbMF.MonitorJustHash = monitoredFile.MonitorJustHash == "Yes";
                    dbMF.MonitorProperties = monitoredFile.MonitorProperties == "Yes";

                    _context.Update(dbMF);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonitoredFileExists(monitoredFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(monitoredFile);
        }

        // GET: Monitor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitoredFile = await _context.MonitoredFiles
                .Include(x => x.Delay)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitoredFile == null)
            {
                return NotFound();
            }

            var viewMF = new MonitoredFileViewModel
            {
                Name = monitoredFile.Name,
                Location = monitoredFile.Location,
                DelayId = monitoredFile.Delay.Id,
                DelayName = monitoredFile.Delay.Name,
                Id = monitoredFile.Id,
                Enabled = monitoredFile.Enabled ? "Yes": "No",
                MonitorJustHash = monitoredFile.MonitorJustHash ? "Yes" : "No",
                MonitorProperties = monitoredFile.MonitorProperties ? "Yes" : "No"
            };

            return View(viewMF);
        }

        // POST: Monitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // TODO: Check for other dependencies here before deleting
            var monitoredFile = await _context.MonitoredFiles.FindAsync(id);
            _context.MonitoredFiles.Remove(monitoredFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonitoredFileExists(int id)
        {
            return _context.MonitoredFiles.Any(e => e.Id == id);
        }


        // GET: Monitor/Edit/5
        public async Task<IActionResult> History(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ;
            var monitoredFile = await _context.MonitoredFiles.Include(x => x.Delay).FirstOrDefaultAsync(x => x.Id == id);
            if (monitoredFile == null)
            {
                return NotFound();
            }

            var monitoredFileView = new MonitoredFileViewModel
            {
                Name = monitoredFile.Name,
                Id = monitoredFile.Id,
                DelayId = monitoredFile.Delay.Id,
                Location = monitoredFile.Location,
                Enabled = monitoredFile.Enabled ? "Yes" : "No",
                MonitorJustHash = monitoredFile.MonitorJustHash ? "Yes" : "No",
                MonitorProperties = monitoredFile.MonitorProperties ? "Yes" : "No"
            };

            var hlist = new List<MonitoredFileHistory>();

            var hashList = await _context.MonitoredFileHashes.Where(x => x.MonitoredFile.Id == id).ToListAsync();

            for (int i = 0; i < hashList.Count; i++)
            {
                hlist.Add(new MonitoredFileHistory
                {
                    ChangeContent = (i == 0) ? "New File HASH Added" : "File HASH changed",
                    ChangeDate = hashList[i].HashDate
                });
            }

            var propList = await _context.MonitoredFileProperties.Where(x => x.MonitoredFile.Id == id).ToListAsync();

            for (int i = 0; i < propList.Count; i++)
            {
                hlist.Add(new MonitoredFileHistory
                {
                    ChangeContent = propList[i].Property,
                    ChangeDate = propList[i].HashDate
                });
            }

            var contentList = await _context.MonitoredFileContents.Where(x => x.MonitoredFile.Id == id).ToListAsync();
            for (int i = 1; i < contentList.Count; i++) // start at the second item
            {
                var linesA = System.Text.Encoding.Default.GetString(contentList[i - 1].Content);
                var linesB = System.Text.Encoding.Default.GetString(contentList[i].Content);
                hlist.Add(new MonitoredFileHistory
                {
                    ChangeContent = string.Concat(linesB.Except(linesA)),
                    ChangeDate = contentList[i].ContentDate
                });
            }

            if (hlist.Count > 0) {
                monitoredFileView.History = hlist.OrderBy(x => x.ChangeDate).ToList();
            }

            //var freqs = new SelectList(_context.MonitorFrequencies.ToList(), "Id", "Name").ToList();
            //ViewBag.MonitorFrequencies = freqs;
            return View(monitoredFileView);
        }
    }
}
