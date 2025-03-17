using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElContadorPampero.Data;
using ElContadorPampero.Models;
using ElContadorPampero.Models.ViewModels;
using System.Data;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ElContadorPampero.Controllers
    
{
    public class UsuariosController : Controller
    {
        private readonly ElContador2025V2Context _context;

        public UsuariosController(ElContador2025V2Context context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }




        public async Task<IActionResult> Salir() {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuarios");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel credenciales) {

            Usuario? usuarioEncontrado = await _context.Usuarios
                                        .Where(u =>
                                            u.Email == credenciales.Email &&
                                            u.Pass == credenciales.Password).FirstOrDefaultAsync();

            if (usuarioEncontrado == null) {
                ViewData["msg"] = "Usuario No encontrado";
                return View();
            }
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Email,usuarioEncontrado.Email),
                new Claim(ClaimTypes.Sid, usuarioEncontrado.Id.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties pro = new AuthenticationProperties() { 
                AllowRefresh = true,
            };
            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    pro
                );
            return RedirectToAction("Index","Home");
        } 

        public IActionResult Login()
        {
            return View();
        }
        // GET: Usuarios/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Registrarse usuario)
        {
            Usuario user = new Usuario();
            if (usuario.Email != usuario.ConfirmarEmail) {
                ViewData["msg"] = "CONFIRMACION DE CORREO INCORRECTA";
                return View();
            }
            user.Email = usuario.Email;
            user.Pass = usuario.Pass;
            user.Apellidos = usuario.Apellidos;
            user.Nombre = usuario.Nombre;
            user.Identificacion = usuario.Identificacion;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identificacion,Apellidos,Nombre,Email,Pass")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
