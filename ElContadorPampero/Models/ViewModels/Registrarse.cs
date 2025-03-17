using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ElContadorPampero.Models.ViewModels
{
    public class Registrarse
    {
        
        public int Id { get; set; }

        public string Identificacion { get; set; }

        public string Apellidos { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }
        public string ConfirmarEmail{ get; set; }
        public string Pass { get; set; }
    }
}
