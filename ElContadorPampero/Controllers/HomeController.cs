using ElContadorPampero.Data;
using ElContadorPampero.Models;
using ElContadorPampero.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ElContadorPampero.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ElContador2025V2Context _logger;

        public HomeController(ElContador2025V2Context logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MayoreoFechas mf)
        {
            var mayor = await _logger.CuentaContables
                .Include(det=>det.DetalleAsientoContables.Where(a=>a.AsientoContable.Fecha<=mf.ff && a.AsientoContable.Fecha>=mf.fi))
                .ThenInclude(c=>c.AsientoContable)
                .OrderBy(g=>g.Tipo)
                .ToListAsync();

            ViewData["mayor"] = mayor;

            var sumadebito = await _logger.DetalleAsientoContables
                                .Where(a => a.AsientoContable.Fecha <= mf.ff && a.AsientoContable.Fecha >= mf.fi && a.Cargo == "Debe")
                                .SumAsync(ss => ss.Monto);

            var sumaHaber = await _logger.DetalleAsientoContables
                                .Where(a => a.AsientoContable.Fecha <= mf.ff && a.AsientoContable.Fecha >= mf.fi && a.Cargo == "Haber")
                                .SumAsync(ss => ss.Monto);

            ViewData["sumadebito"] = sumadebito;
            ViewData["sumaHaber"] = sumaHaber;
                
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
