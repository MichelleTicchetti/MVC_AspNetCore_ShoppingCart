using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Carrito_A.Helpers;

namespace Carrito_A.Models
{
    public class Carrito
    {

        public Carrito()
        {
            this.Activo = true;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public bool Activo { get; set; }

        //Propiedad Navegacional
        public Cliente Cliente { get; set; }

        //Propiedad Navegacional
        public List<CarritoItem> CarritoItems { get; set; }

        [DataType(DataType.Currency)]
        public double SubTotal { get { return calcularSubtotal(); } }

        private double calcularSubtotal()
        {
            double acum = 0;

            if(CarritoItems != null && CarritoItems.Count > 0)
            {
                foreach(CarritoItem item in CarritoItems)
                {
                    double subTotal = item.SubTotal;
                    acum += subTotal;
                }
            }

            return acum;
        }

        //Propiedad Relacional con Cliente (1 a 1). La entidad de fuerza es Cliente. Si se elimina el Cliente, se elimina el Carrito. Por lo tanto, la prop.relac. se define acá.  
        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.CLIENTE)]
        public int ClienteId { get; set; }



    }
}

