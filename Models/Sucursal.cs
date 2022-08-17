using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Carrito_A.Helpers;


namespace Carrito_A.Models
{
    public class Sucursal
    {

        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        public string Nombre { get; set; }

        // Propiedad Navegacional
        // public Direccion Direccion { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.PhoneNumber, ErrorMessage = MensajesError.TelefonoInvalido)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.Email)]
        [DataType(DataType.EmailAddress, ErrorMessage = MensajesError.EmailInvalido)]
        public string Email { get; set; }

        // Propiedad Navegacional
        public List<StockItem> StockItems { get; set; }

        // Propiedad Navegacional
        public List<Compra> Compras { get; set; }

    }
}
