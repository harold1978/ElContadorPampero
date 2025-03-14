using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElContadorPampero.Data;
using ElContadorPampero.Models;

namespace ElContadorPampero.Controllers
{
    public class ContabilidadsController : Controller
    {
        private readonly ElContador2025V2Context _context;

        public ContabilidadsController(ElContador2025V2Context context)
        {
            _context = context;
        }

        // GET: Contabilidads
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contabilidads.ToListAsync());
        }

        // GET: Contabilidads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contabilidad = await _context.Contabilidads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contabilidad == null)
            {
                return NotFound();
            }

            return View(contabilidad);
        }

        // GET: Contabilidads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contabilidads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaCreacion,Nombre,FechaInicioPeriodo,FechaFinalPeriodo,Empresa")] Contabilidad contabilidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contabilidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contabilidad);
        }

        // GET: Contabilidads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contabilidad = await _context.Contabilidads.FindAsync(id);
            if (contabilidad == null)
            {
                return NotFound();
            }
            return View(contabilidad);
        }

        // POST: Contabilidads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCreacion,Nombre,FechaInicioPeriodo,FechaFinalPeriodo,Empresa")] Contabilidad contabilidad)
        {
            if (id != contabilidad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contabilidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContabilidadExists(contabilidad.Id))
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
            return View(contabilidad);
        }

        // GET: Contabilidads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contabilidad = await _context.Contabilidads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contabilidad == null)
            {
                return NotFound();
            }

            return View(contabilidad);
        }

        // POST: Contabilidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contabilidad = await _context.Contabilidads.FindAsync(id);
            if (contabilidad != null)
            {
                _context.Contabilidads.Remove(contabilidad);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContabilidadExists(int id)
        {
            return _context.Contabilidads.Any(e => e.Id == id);
        }
    }
}
