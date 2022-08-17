using System.ComponentModel.DataAnnotations;
using Carrito_A.Helpers;


namespace Carrito_A.Models
{
    public class Direccion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Localidad { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Calle { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = MensajesError.RangoMinimo)]
        [Required(ErrorMessage = MensajesError.Requerido)]
        public int Numero { get; set; }

        //aparece como required??
        public int Piso { get; set; }
         
        public string Departamento { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int CodigoPostal { get; set; }

        public Persona Persona { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public int PersonaId { get; set; }

        //public Sucursal Sucursal { get; set; }

        //[Required(ErrorMessage = MensajesError.Requerido)]
        //public int SucursalId { get; set; }

        public string DireccionCompleta
        {
            get
            {
                return $"{Calle} {Numero}, Piso: {Piso}, Dpto: {Departamento}, Cod. Postal: {CodigoPostal}, Localidad: {Localidad}";
            }
        }

    }
}
