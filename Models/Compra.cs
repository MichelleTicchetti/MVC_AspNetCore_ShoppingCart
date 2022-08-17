

using Carrito_A.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Carrito_A.Models
{
    public class Compra
    {

        //Constructor por default
        //public Compra() { }

        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string CodCompra { get; set; } = Configuraciones.NuevaCompra(Restricciones.CompraMax);

        public Cliente Cliente { get; set; }
        public Carrito Carrito { get; set; }

        public Sucursal Sucursal { get; set; }

        [DataType(DataType.Currency)]
        public double Total { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int SucursalId { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int CarritoId { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
    }
}
