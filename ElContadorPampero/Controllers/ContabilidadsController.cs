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
using System.Security.Claims;

namespace ElContadorPampero.Controllers
{
    [Authorize]
    public class ContabilidadsController : Controller
    {
        private readonly ElContador2025V2Context _context;
        private IUsuario _UsuarioId;
        public ContabilidadsController(ElContador2025V2Context context, IUsuario iusuario)
        {
            _context = context;
            _UsuarioId = iusuario;
        }

        // GET: Contabilidads
        public async Task<IActionResult> Index()
        {
            var elContador2025V2Context = _context.Contabilidads.Include(c => c.Usuario).Where(r => r.UsuarioId == int.Parse(_UsuarioId.GetUsuarioId()));

            return View(await elContador2025V2Context.ToListAsync());
        }

        // GET: Contabilidads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contabilidad = await _context.Contabilidads
                .Include(c => c.Usuario)
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
            ViewData["UsuarioId"] = _UsuarioId.GetUsuarioId(); //new SelectList(_context.Usuarios,"Id", "Apellidos",_UsuarioId.GetUsuarioId());
            return View();
        }

        // POST: Contabilidads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaCreacion,Nombre,FechaInicioPeriodo,FechaFinalPeriodo,Empresa,UsuarioId")] Contabilidad contabilidad)
        {
            if (ModelState.IsValid)
            {

                _context.Add(contabilidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), contabilidad.UsuarioId);
            }
            ViewData["UsuarioId"] = _UsuarioId.GetUsuarioId();
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
            ViewData["UsuarioId"] = _UsuarioId.GetUsuarioId();
            return View(contabilidad);
        }

        // POST: Contabilidads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCreacion,Nombre,FechaInicioPeriodo,FechaFinalPeriodo,Empresa,UsuarioId")] Contabilidad contabilidad)
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Apellidos", _UsuarioId.GetUsuarioId());
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
                .Include(c => c.Usuario)
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
