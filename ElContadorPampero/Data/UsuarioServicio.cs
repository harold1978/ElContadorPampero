using ElContadorPampero.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ElContadorPampero.Data
{

    public interface IUsuario {
        public void SetUsuarioId(string? id);
        public string GetUsuarioId();
        public void SetContabilidadId(int id);
        public int GetContabilidadId();

        public void SetCorreo(string correo);
        public string GetCorreo();

    }
    public class UsuarioServicio : IUsuario
    {
        public string? _UsuarioId { get; set; }

        public int _ContabilidadId { get; set; }

        public string? _correo { get;set; }

        public int GetContabilidadId()
        {
            return _ContabilidadId;
        }

        public string GetCorreo()
        {
            return _correo;
        }

        public string GetUsuarioId()
        {
            return _UsuarioId!;
        }

        public void SetContabilidadId(int id)
        {
            _ContabilidadId = id;
        }

        public void SetCorreo(string correo)
        {
            _correo = correo;
        }

        public void SetUsuarioId(string? id)
        {
            
            _UsuarioId = id!=null ? id :"Usuario no registrado" ;
        }

    


    }
}
