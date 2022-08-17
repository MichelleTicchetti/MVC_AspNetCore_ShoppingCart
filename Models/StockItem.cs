using System.ComponentModel.DataAnnotations;
using Carrito_A.Helpers;
using System;


namespace Carrito_A.Models
{
    public class StockItem
    {

        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int SucursalId {get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int ProductoId { get; set; }

        public Sucursal Sucursal { get; set; }

        public Producto Producto { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(Restricciones.StockMin, Restricciones.StockMax, ErrorMessage = MensajesError.RangoMinimoMaximo)]
        public int Cantidad { get; set; }


    }
}
