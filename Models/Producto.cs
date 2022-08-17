using System.ComponentModel.DataAnnotations;
using Carrito_A.Helpers;
using System;


namespace Carrito_A.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        [Display(Name = Alias.NOMBRE_PRODUCTO)]
        public string Nombre { get; set; }

        [StringLength(200, MinimumLength = 5, ErrorMessage = MensajesError.StrMaxMin)]
        [DataType(DataType.MultilineText)]
        [Display(Name = Alias.DESCRIPCION_PRODUCTO)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(0, double.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [DataType(DataType.Currency)]
        [Display(Name = Alias.PRECIO_VIGENTE)]
        public double PrecioVigente { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public bool Activo { get; set; }

        //Propiedad Navegacional
        public Categoria Categoria { get; set; }

        //Propiedad Relacional con Categoria (1 a n). La entidad de fuerza es Categoria. Si se elimina la Categoria, se elimina el Producto. Por lo tanto, la prop.relac. se define acá. 
        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.CATEGORIA)]
        public int CategoriaId { get; set; }

        public string Foto { get; set; } = Configuraciones.ProductoDef;


    }
}
