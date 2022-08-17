using Carrito_A.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_A.ViewModels
{
    public class Login
    {

        [Required(ErrorMessage = MensajesError.Requerido)]
        [EmailAddress(ErrorMessage = MensajesError.NoValido)]
        public string Email { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        public bool Recordarme { get; set; } = false;
    }

}
