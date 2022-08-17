using Carrito_A.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.ViewModels
{
    public class DireccionTelefono
    {

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Localidad { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Calle { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public int Numero { get; set; }

        //aparece como required??
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
