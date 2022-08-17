using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.Helpers
{
    public class Configuraciones
    {
        private static string todosLosCaracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        public const string PassPorDefecto = "Password1!";
        public const string ProductosPATH = "img\\fotos\\productos";
        public const string ProductoDef = "productodefault.jpg";
        public const string ClientesPATH = "img\\fotos\\clientes";
        public const string EmpleadosPATH = "img\\fotos\\empleados";
        public const string PersonaDef = "personadefault.png"; 
        public static readonly List<string> FotosClientes = new List<string>() { "cliente1.jpeg", "cliente2.jpg", "cliente3.jpg", "cliente4.jpg", "cliente5.jpg", "cliente6.jpg", "cliente7.jpg", "cliente8.jpg", "cliente9.jpeg", "cliente10.jpeg" };



        public static string CaracteresRandom(int largo)
        {
            return new string(Enumerable.Repeat(todosLosCaracteres, largo).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string NuevaCompra(int largo)
        {
            return new string(Enumerable.Repeat(todosLosCaracteres, largo).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
