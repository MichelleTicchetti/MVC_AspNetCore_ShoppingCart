using Carrito_A.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Carrito_A.Models
{
    public class Persona : IdentityUser<int>
    {
        

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        public string Apellido { get; set; }

        //[Required(ErrorMessage = MensajesError.Requerido)]
        //[Display(Name = Alias.Email)]
        //[EmailAddress(ErrorMessage = MensajesError.EmailInvalido)]
        //public override string Email { get; set; }

        //[Required(ErrorMessage = MensajesError.Requerido)]
        //[Display(Name = Alias.UserName)]
        //public override string UserName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = Alias.FechaHoraAlta)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.Password, ErrorMessage = MensajesError.PasswordInvalida)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        public Direccion Direccion { get; set; }

        public Telefono Telefono { get; set; }

        public string Foto { get; set; } = Configuraciones.PersonaDef;

        public string NombreCompleto
        {
            get
            {
                if (string.IsNullOrEmpty(Apellido)) return Nombre;
                if (string.IsNullOrEmpty(Nombre)) return Apellido;
                return $"{Apellido.ToUpper()}, {Nombre}";
            }
        }

    }
}
