using System.ComponentModel.DataAnnotations;
using Carrito_A.Helpers;
using System;


namespace Carrito_A.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }

        //Propiedad Navegacional
        public Carrito Carrito { get; set; }

        //Propiedad Relacional con Carrito (1 a m). La entidad de fuerza es Carrito. Si se elimina el Carrito, se eliminan los Items del carrito. Por lo tanto, la prop.relac. se define acá. 
        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.CARRITO)]
        public int CarritoId { get; set; }

        //Propiedad Navagacional
        public Producto Producto { get; set; }

        //Propiedad Relacional con Producto (1 a 1). La entidad de fuerza es Producto. Si se elimina el Producto, se elimina el Item del carrito. Por lo tanto, la prop.relac. se define acá. 
        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.PRODUCTO)]
        public int ProductoId { get; set; }

        /*[Range(0, double.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]*/
        [DataType(DataType.Currency)]
        [Display(Name = Alias.VALOR_UNITARIO)]
        public double ValorUnitario { get; set;}

        //public double ValorUnitario { get {return this.ValorUnitario; } set { getValorProducto(); } }
        //private double getValorProducto()
        //{
        //    double valorUnitario = 0;

        //    if(Producto != null)
        //    {
        //        valorUnitario = Producto.PrecioVigente;
        //    }

        //    return valorUnitario;
        //}

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public int Cantidad { get; set; }

        /*[Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]*/
        [DataType(DataType.Currency)]
        public double SubTotal { get { return calcularSubtotal(); }  }

        private double calcularSubtotal()
        {
            return Cantidad * ValorUnitario;
        }
    }


}
