using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.Helpers
{
    public class Alias
    {
        public const string NOMBRE_PRODUCTO = "Nombre del Producto";
        public const string DESCRIPCION_PRODUCTO = "Descripción";
        public const string PRECIO_VIGENTE = "Precio Vigente";
        public const string CATEGORIA = "Categoría";
        public const string VALOR_UNITARIO = "Valor Unitario";
        public const string PRODUCTO = "Producto";
        public const string CARRITO = "Carrito";
        public const string CLIENTE = "Cliente";
        public const string CARRITOITEMS = "Items del Carrito";
        public const string FechaHoraAlta = "Fecha y hora del alta";
        public const string Email = "Correo electrónico";
        public const string DNI = "Documento de Identidad";
        public const string UserName = "Nombre de Usuario";
        public const string Password = "Contraseña";
        public const string RolName = "Nombre";
        public const string PassConfirmacion = "Confirmación de contraseña";
        public const string Telefono = "Teléfono";
    }

    public static class AliasGUI
    {
        public static string Create { get { return "Crear"; } }
        public static string Delete { get { return "Eliminar"; } }
        public static string Edit { get { return "Editar"; } }
        public static string Details { get { return "Detalles"; } }
        public static string Back { get { return "Volver atras"; } }
        public static string BackToList { get { return "Volver al listado"; } }
        public static string SureToDelete { get { return "¿Está seguro que desa proceder con la eliminación?"; } }
        public static string ListOf { get { return "Listado de "; } }
        public static string Save { get { return "Guardar"; } }
        public static string SeleccionarImagen { get { return "Seleccionar imagen"; } }
        public static string SubirImagen { get { return "Subir imagen"; } }

        public static string EliminarImagen { get { return "Eliminar imagen"; } }


    }
}
