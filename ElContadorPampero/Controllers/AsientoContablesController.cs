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
    public class AsientoContablesController : Controller
    {
        private readonly ElContador2025V2Context _context;
        private readonly IUsuario _usuario;

        public AsientoContablesController(ElContador2025V2Context context, IUsuario usuario)
        {
            _context = context;
            _usuario = usuario;
        }

        // GET: AsientoContables
        public async Task<IActionResult> Index(int? id)
        {
            if (id!=null) {
                _usuario.SetContabilidadId(id.Value);
            }
            
            var elContador2025V2Context = _context.AsientoContables
                .Include(a => a.Contabilidad)
                .Include(a => a.Usuario)
                .Include(e=>e.DetalleAsientoContables)
                .Where(idu => idu.ContabilidadId == _usuario.GetContabilidadId() && idu.UsuarioId == _usuario.GetUsuarioId());

            var lista = await _context.DetalleAsientoContables
                        .Include(r=>r.AsientoContable)
                        .Where(h=>h.AsientoContable.ContabilidadId == _usuario.GetContabilidadId() &&
                        h.AsientoContable.UsuarioId == _usuario.GetUsuarioId()).ToListAsync();

            //var totaldebe =elContador2025V2Context.
            //var totalHaber = elContador2025V2Context.Where(t => t.Cargo == "Haber").Sum(o => o.Monto);
            ViewBag.listaV = lista;
            return View(await elContador2025V2Context.ToListAsync());
        }

        // GET: AsientoContables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asientoContable = await _context.AsientoContables
                .Include(a => a.Contabilidad)
                .Include(a => a.Usuario)
                .Include(a => a.DetalleAsientoContables).ThenInclude(a=>a.CuentaContable)
                .FirstOrDefaultAsync(idu => idu.Id == id);
                
            if (asientoContable == null)
            {
                return NotFound();
            }

            return View(asientoContable);
        }

        // GET: AsientoContables/Create
        public IActionResult Create()
        {
            ViewData["ContabilidadId"] = new SelectList(_context.Contabilidads.Where(idu => idu.UsuarioId == _usuario.GetUsuarioId()), "Id", "Empresa",_usuario.GetContabilidadId());
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.Where(idu => idu.Id == _usuario.GetUsuarioId()), "Id", "Apellidos");
            return View();
        }

        // POST: AsientoContables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Detalle,NroAsiento,ContabilidadId,UsuarioId")] AsientoContable asientoContable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asientoContable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContabilidadId"] = new SelectList(_context.Contabilidads.Where(idu => idu.UsuarioId == _usuario.GetUsuarioId()), "Id", "Empresa", _usuario.GetContabilidadId());
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.Where(idu => idu.Id == _usuario.GetUsuarioId()), "Id", "Apellidos", _usuario.GetUsuarioId());
            return RedirectToAction(nameof(Index));
        }

        // GET: AsientoContables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asientoContable = await _context.AsientoContables.FindAsync(id);
            if (asientoContable == null)
            {
                return NotFound();
            }
            ViewData["ContabilidadId"] = new SelectList(_context.Contabilidads.Where(idu => idu.UsuarioId == _usuario.GetUsuarioId()), "Id", "Empresa", asientoContable.ContabilidadId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.Where(idu => idu.Id == _usuario.GetUsuarioId()), "Id", "Apellidos", asientoContable.UsuarioId);
            return View(asientoContable);
        }

        // POST: AsientoContables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Detalle,NroAsiento,ContabilidadId,UsuarioId")] AsientoContable asientoContable)
        {
            if (id != asientoContable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asientoContable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsientoContableExists(asientoContable.Id))
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
            ViewData["ContabilidadId"] = new SelectList(_context.Contabilidads.Where(idu => idu.UsuarioId == _usuario.GetUsuarioId()), "Id", "Empresa", asientoContable.ContabilidadId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.Where(idu => idu.Id == _usuario.GetUsuarioId()), "Id", "Apellidos", asientoContable.UsuarioId);
            return View(asientoContable);
        }

        // GET: AsientoContables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asientoContable = await _context.AsientoContables
                .Include(a => a.Contabilidad)
                .Include(a => a.Usuario)
                .Where(idu => idu.ContabilidadId == id && idu.UsuarioId == _usuario.GetUsuarioId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asientoContable == null)
            {
                return NotFound();
            }

            return View(asientoContable);
        }

        // POST: AsientoContables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asientoContable = await _context.AsientoContables.FindAsync(id);
            if (asientoContable != null)
            {
                _context.AsientoContables.Remove(asientoContable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsientoContableExists(int id)
        {
            return _context.AsientoContables.Any(e => e.Id == id);
        }
    }
}
