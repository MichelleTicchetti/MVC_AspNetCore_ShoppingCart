using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Carrito_A.Helpers;

namespace Carrito_A.Models
{
    public class Categoria
    {

        public Categoria()
        {
            this.Productos = new List<Producto>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(40, MinimumLength = 4, ErrorMessage = MensajesError.StrMaxMin)]
        public string Nombre { get; set; }


        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        public List<Producto> Productos { get; set; }


    }
}
