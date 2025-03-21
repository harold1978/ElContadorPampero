using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElContadorPampero.Data;
using ElContadorPampero.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElContadorPampero.Controllers
{
    [Authorize]
    public class DetalleAsientoContablesController : Controller
    {
        private readonly ElContador2025V2Context _context;

        public DetalleAsientoContablesController(ElContador2025V2Context context)
        {
            _context = context;
        }

        // GET: DetalleAsientoContables
        public async Task<IActionResult> Index(int id)
        {
            var elContador2025V2Context = await _context.DetalleAsientoContables
                .Include(d => d.AsientoContable)
                .Include(d => d.CuentaContable)
                .Where(w=>w.AsientoContableId == id).ToListAsync();

            return View(elContador2025V2Context);
        }

        // GET: DetalleAsientoContables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleAsientoContable = await _context.DetalleAsientoContables
                .Include(d => d.AsientoContable)
                .Include(d => d.CuentaContable)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleAsientoContable == null)
            {
                return NotFound();
            }

            return View(detalleAsientoContable);
        }

        // GET: DetalleAsientoContables/Create
        public IActionResult Create()
        {
            ViewData["AsientoContableId"] = new SelectList(_context.AsientoContables, "Id", "Detalle");
            ViewData["CuentaContableId"] = new SelectList(_context.CuentaContables, "Id", "Codigo");
            return View();
        }

        // POST: DetalleAsientoContables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AsientoContableId,CuentaContableId,Cargo,Monto")] DetalleAsientoContable detalleAsientoContable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleAsientoContable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AsientoContableId"] = new SelectList(_context.AsientoContables, "Id", "Detalle", detalleAsientoContable.AsientoContableId);
            ViewData["CuentaContableId"] = new SelectList(_context.CuentaContables, "Id", "Codigo", detalleAsientoContable.CuentaContableId);
            return View(detalleAsientoContable);
        }

        // GET: DetalleAsientoContables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleAsientoContable = await _context.DetalleAsientoContables.FindAsync(id);
            if (detalleAsientoContable == null)
            {
                return NotFound();
            }
            ViewData["AsientoContableId"] = new SelectList(_context.AsientoContables, "Id", "Detalle", detalleAsientoContable.AsientoContableId);
            ViewData["CuentaContableId"] = new SelectList(_context.CuentaContables, "Id", "Codigo", detalleAsientoContable.CuentaContableId);
            return View(detalleAsientoContable);
        }

        // POST: DetalleAsientoContables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AsientoContableId,CuentaContableId,Cargo,Monto")] DetalleAsientoContable detalleAsientoContable)
        {
            if (id != detalleAsientoContable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleAsientoContable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleAsientoContableExists(detalleAsientoContable.Id))
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
            ViewData["AsientoContableId"] = new SelectList(_context.AsientoContables, "Id", "Detalle", detalleAsientoContable.AsientoContableId);
            ViewData["CuentaContableId"] = new SelectList(_context.CuentaContables, "Id", "Codigo", detalleAsientoContable.CuentaContableId);
            return View(detalleAsientoContable);
        }

        // GET: DetalleAsientoContables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleAsientoContable = await _context.DetalleAsientoContables
                .Include(d => d.AsientoContable)
                .Include(d => d.CuentaContable)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleAsientoContable == null)
            {
                return NotFound();
            }

            return View(detalleAsientoContable);
        }

        // POST: DetalleAsientoContables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleAsientoContable = await _context.DetalleAsientoContables.FindAsync(id);
            if (detalleAsientoContable != null)
            {
                _context.DetalleAsientoContables.Remove(detalleAsientoContable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleAsientoContableExists(int id)
        {
            return _context.DetalleAsientoContables.Any(e => e.Id == id);
        }
    }
}
