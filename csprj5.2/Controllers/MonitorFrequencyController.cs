using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using csprj5._2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace csprj5._2.Controllers
{
    public class MonitorFrequencyController : Controller
    {
        private readonly DbContext _context;

        public MonitorFrequencyController(DbContext context)
        {
            _context = context;
        }

        // GET: List of Monitor Frequencies
        // GET: MF
        public async Task<IActionResult> Index()
        {
            return View(await _context.MonitorFrequencies
                .ToListAsync());
        }

        // GET: MF/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mF = await _context.MonitorFrequencies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mF == null)
            {
                return NotFound();
            }

            mF.MonitoredFiles = await _context.MonitoredFiles
                .Where(x => x.Delay.Id == mF.Id).ToListAsync();

            return View(mF);
        }

        // GET: MF/Create - this is landing page for the create
        public IActionResult Create()
        {
            return View();
        }


        // POST: MF/Create - sending back the entries for saving
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DelayInSecs")] MonitorFrequency mF)
        {
            if (ModelState.IsValid)
            {
                MonitorFrequency dbMF = _context.Add(new MonitorFrequency
                {
                    Name = mF.Name,
                    DelayInSecs = mF.DelayInSecs
                }).Entity;
                await _context.SaveChangesAsync();
                mF.Id = dbMF.Id;
                return RedirectToAction(nameof(Index));
            }
            return View(mF);
        }

        // GET: MF/Edit/5 - editing a specific MF
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mF = await _context.MonitorFrequencies
                .FindAsync(id);
            if (mF == null)
            {
                return NotFound();
            }
            return View(mF);
        }

        // POST: MF/Edit/5 - saving the edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DelayInSecs")] MonitorFrequency mF)
        {
            if (id != mF.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mF);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonitorFrequencyExists(mF.Id))
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
            return View(mF);
        }

        // GET: MF/Delete/5 - landing page for the delete
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: MF/Delete/5 - delete an MF
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mF = await _context.MonitorFrequencies.FindAsync(id);

            if (mF == null)
            {
                return NotFound();
            }

            if (_context.MonitoredFiles.Count(x => x.Delay.Id == mF.Id) == 0)
            {
                _context.MonitorFrequencies.Remove(mF);
                await _context.SaveChangesAsync();
            }
            // check if any files are using this MF
            return RedirectToAction(nameof(Index));
        }

        private bool MonitorFrequencyExists(int id)
        {
            return _context.MonitorFrequencies.Any(e => e.Id == id);
        }

    }
}
