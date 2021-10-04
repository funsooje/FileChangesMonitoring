using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using csprj5._2.Models;

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
            return View(await _context.MonitoredFiles.ToListAsync());
        }

        // GET: Monitor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitoredFile = await _context.MonitoredFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitoredFile == null)
            {
                return NotFound();
            }

            return View(monitoredFile);
        }

        // GET: Monitor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Monitor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location")] MonitoredFile monitoredFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monitoredFile);
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

            var monitoredFile = await _context.MonitoredFiles.FindAsync(id);
            if (monitoredFile == null)
            {
                return NotFound();
            }
            return View(monitoredFile);
        }

        // POST: Monitor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location")] MonitoredFile monitoredFile)
        {
            if (id != monitoredFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monitoredFile);
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitoredFile == null)
            {
                return NotFound();
            }

            return View(monitoredFile);
        }

        // POST: Monitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monitoredFile = await _context.MonitoredFiles.FindAsync(id);
            _context.MonitoredFiles.Remove(monitoredFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonitoredFileExists(int id)
        {
            return _context.MonitoredFiles.Any(e => e.Id == id);
        }
    }
}
