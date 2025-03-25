using ElContadorPampero.Data;
using ElContadorPampero.Models;
using ElContadorPampero.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.RateLimiting;

namespace ElContadorPampero.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ElContador2025V2Context _logger;
        private readonly IUsuario _usuario;
        public HomeController(ElContador2025V2Context logger, IUsuario usuario)
        {
            _logger = logger;
            _usuario = usuario;
        }

        public IActionResult Index(int CId)
        {
            _usuario.SetContabilidadId(CId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MayoreoFechas mf)
        {
                 
            
            var mayor = await _logger.CuentaContables
                                    .Include(q=>q.DetalleAsientoContables
                                                    .Where(tt => tt.AsientoContable.Fecha <= mf.ff 
                                                        && tt.AsientoContable.Fecha >= mf.fi
                                                        && tt.AsientoContable.ContabilidadId == _usuario.GetContabilidadId()
                                                        && tt.AsientoContable.UsuarioId == _usuario.GetUsuarioId()))
                                    .ThenInclude(qq=>qq.AsientoContable)
                                    .ToListAsync();

            var mayor1 = mayor.Where(tt => tt.DetalleAsientoContables.Any()).OrderBy(g => g.Tipo).ToList();
            
    //.Where(t => t.DetalleAsientoContables.Where(tt => tt.AsientoContable.Fecha <= mf.ff && tt.AsientoContable.Fecha >= mf.fi &&
    //                                            tt.AsientoContable.ContabilidadId == _usuario.GetContabilidadId() &&
    //                                            tt.AsientoContable.UsuarioId == _usuario.GetUsuarioId()))


            //var uno = await _logger.AsientoContables
            //    .Include(b=>b.DetalleAsientoContables)
            //    .ThenInclude(bc=>bc.CuentaContable)
            //    .Where(g=>g.DetalleAsientoContables.Count>0 
            //        && g.UsuarioId == _usuario.GetUsuarioId()
            //        && g.ContabilidadId == _usuario.GetContabilidadId()
            //        && g.Fecha <= mf.ff
            //        && g.Fecha >= mf.fi)
            //    .ToListAsync();

            //ViewBag.pp = uno;      


            //var mayor = await _logger.CuentaContables
            //    //.Where(re => re.DetalleAsientoContables.Count>=0)
            //    .Include(det=>det.DetalleAsientoContables
            //            .Where(a => a.AsientoContable.Fecha<=mf.ff 
            //            && a.AsientoContable.Fecha>=mf.fi 
            //            && a.AsientoContable.ContabilidadId == _usuario.GetContabilidadId()
            //            && a.AsientoContable.UsuarioId == _usuario.GetUsuarioId()))
            //    .ThenInclude(c=>c.AsientoContable)
            //    .Where(ff=>ff.DetalleAsientoContables.Count>0)
            //    .OrderBy(g=>g.Tipo)
            //    .ToListAsync();

            ViewData["mayor"] = mayor1;

            //var sumadebito = await _logger.DetalleAsientoContables
            //                    .Where(a => a.AsientoContable.Fecha <= mf.ff 
            //                    && a.AsientoContable.Fecha >= mf.fi 
            //                    && a.Cargo == "Debe"
            //                    && a.AsientoContable.ContabilidadId == _usuario.GetContabilidadId()
            //                    && a.AsientoContable.UsuarioId == _usuario.GetUsuarioId())
            //                    .SumAsync(ss => ss.Monto);

            var sumadebito = mayor.SelectMany(m => m.DetalleAsientoContables, (a, b) => new { b.Cargo, b.Monto })
                                    .Where(m=>m.Cargo=="Debe").Sum(m=>m.Monto);

            var sumaHaber = mayor.SelectMany(m => m.DetalleAsientoContables, (a, b) => new { b.Cargo, b.Monto })
                                    .Where(m => m.Cargo == "Haber").Sum(m => m.Monto);//await _logger.DetalleAsientoContables
            //                    .Where(a => a.AsientoContable.Fecha <= mf.ff 
            //                    && a.AsientoContable.Fecha >= mf.fi 
            //                    && a.Cargo == "Haber"
            //                    && a.AsientoContable.ContabilidadId == _usuario.GetContabilidadId()
            //                    && a.AsientoContable.UsuarioId == _usuario.GetUsuarioId())
            //                    .SumAsync(ss => ss.Monto);

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
