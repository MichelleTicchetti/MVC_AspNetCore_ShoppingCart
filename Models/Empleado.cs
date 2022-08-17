
using Carrito_A.Helpers;
using System.ComponentModel.DataAnnotations;


namespace Carrito_A.Models
{
    public class Empleado : Persona
    {

        [Required(ErrorMessage = MensajesError.Requerido)]
        public string Legajo { get; set; } = Configuraciones.CaracteresRandom(Restricciones.LegajoMax);

    }
}
