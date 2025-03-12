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
    public class CuentaContablesController : Controller
    {
        private readonly ElContador2025V2Context _context;

        public CuentaContablesController(ElContador2025V2Context context)
        {
            _context = context;
        }

        // GET: CuentaContables
        public async Task<IActionResult> Index()
        {
            return View(await _context.CuentaContables.ToListAsync());
        }

        // GET: CuentaContables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentaContable = await _context.CuentaContables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuentaContable == null)
            {
                return NotFound();
            }

            return View(cuentaContable);
        }

        // GET: CuentaContables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CuentaContables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nombre,Tipo,Saldo")] CuentaContable cuentaContable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuentaContable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cuentaContable);
        }

        // GET: CuentaContables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentaContable = await _context.CuentaContables.FindAsync(id);
            if (cuentaContable == null)
            {
                return NotFound();
            }
            return View(cuentaContable);
        }

        // POST: CuentaContables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Tipo,Saldo")] CuentaContable cuentaContable)
        {
            if (id != cuentaContable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuentaContable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaContableExists(cuentaContable.Id))
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
            return View(cuentaContable);
        }

        // GET: CuentaContables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentaContable = await _context.CuentaContables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuentaContable == null)
            {
                return NotFound();
            }

            return View(cuentaContable);
        }

        // POST: CuentaContables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuentaContable = await _context.CuentaContables.FindAsync(id);
            if (cuentaContable != null)
            {
                _context.CuentaContables.Remove(cuentaContable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuentaContableExists(int id)
        {
            return _context.CuentaContables.Any(e => e.Id == id);
        }
    }
}
