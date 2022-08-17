using Carrito_A.Helpers;
using System.ComponentModel.DataAnnotations;


namespace Carrito_A.ViewModels
{
    public class RegistracionViewModel
    {

        [Required(ErrorMessage = MensajesError.Requerido)]
        [EmailAddress(ErrorMessage = MensajesError.NoValido)]
        [Display(Name = Alias.Email)]
        //[Remote(action: "EmailDisponible", controller:"Account")]
        public string Email { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.Password, ErrorMessage = MensajesError.NoValido)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.Password, ErrorMessage = MensajesError.NoValido)]
        [Compare("Password", ErrorMessage = MensajesError.PassMissmatch)]
        [Display(Name = Alias.PassConfirmacion)]
        public string ConfirmacionPassword { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = MensajesError.StrMaxMin)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(Restricciones.MinimoDNI, Restricciones.MaximoDNI, ErrorMessage = MensajesError.RangoMinimoMaximo)]
        [Display(Name = Alias.DNI)]
        public int Dni { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Localidad { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Calle { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public int Numero { get; set; }

        //aparece como required??
        public int? Piso { get; set; }

        public string Departamento { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int CodigoPostal { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int CodigoDeArea { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public float NumeroTelefono { get; set; }

    }
}
