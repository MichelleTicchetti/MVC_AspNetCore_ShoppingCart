using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.Helpers
{
    public class MensajesError
    {
        public const string Requerido = "El campo {0} es requerido";
        public const string RangoMinimo = "El campo {0} no puede ser menor a {1}";
        public const string RangoMinimoMaximo = "El campo {0} no puede ser menor a {1}, ni mayor a {2}.";
        public const string StrMaxMin = "El campo {0}, debe tener un mínimo de {2} y un máximo de {1}";
        public const string EmailInvalido = "Formato de Email invalido.";
        public const string PasswordInvalida = "Contraseña invalida.";
        public const string SinStock = "El producto {0} se encuentra fuera de stock";
        public const string NoValido = "El campo {0} no es válido";
        public const string PassMissmatch = "El campo {0} no coincide";
        public const string TelefonoInvalido = "Formato de número de teléfono invalido";
    }
}
