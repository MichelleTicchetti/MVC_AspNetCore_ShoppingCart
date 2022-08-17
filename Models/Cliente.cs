using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Carrito_A.Helpers;
using System;


namespace Carrito_A.Models
{
    public class Cliente : Persona
    {
        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(Restricciones.MinimoDNI, Restricciones.MaximoDNI, ErrorMessage = MensajesError.RangoMinimoMaximo)]
        [Display(Name = Alias.DNI)]
        public int Dni { get; set; }

        public List<Compra> Compras { get; set; }

        public List<Carrito> Carritos { get; set; }

    }
}
