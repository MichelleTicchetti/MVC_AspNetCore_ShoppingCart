using System.ComponentModel.DataAnnotations;
using Carrito_A.Helpers;


namespace Carrito_A.Models
{
    public class Telefono
    {
        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int CodigoDeArea { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public float Numero { get; set; }

        public Persona Persona { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int PersonaId { get; set; }

        //public Sucursal Sucursal { get; set; }

        //[Required(ErrorMessage = MensajesError.Requerido)]
        //public int SucursalId { get; set; }

        public string TelefonoCompleto
        {
            get
            {
                return $"({CodigoDeArea}) - {Numero}";
            }
        }

    }
}
