using Carrito_A.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.ViewModels
{
    public class EditarDatosEmpleado
    {
        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Localidad { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Calle { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public int Numero { get; set; }

        public int Piso { get; set; }

        public string Departamento { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int CodigoPostal { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int CodigoDeArea { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public int NumeroTelefono { get; set; }


    }
}
