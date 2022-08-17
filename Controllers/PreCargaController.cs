using Carrito_A.Data;
using Carrito_A.Helpers;
using Carrito_A.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.Controllers
{
    public class PreCargaController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _rolManager;
        private readonly CarritoContext _carritoContext;
        private const string passDefault = "Password1!";

        

       public PreCargaController(UserManager<Persona> userManager, RoleManager<Rol> rolManager, CarritoContext contexto)
        {
            _userManager = userManager;
            _rolManager = rolManager;
            _carritoContext = contexto;
        }

        public IActionResult Arranque()
        {
            try
            {
                _carritoContext.Database.EnsureDeleted();
                _carritoContext.Database.Migrate();
                CrearRolesBase().Wait();
                CrearAdmin().Wait();
                CrearEmpleado().Wait();
                CrearClientes().Wait();
                CrearSucursales().Wait();
                CrearCategorias().Wait();
                CrearProductos().Wait();
                CrearStock().Wait();
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

            return RedirectToAction("Index", "Home");
        }

            public IActionResult Index()
        {
            return View();
        }

        private async Task CrearRolesBase()
        {
            List<string> roles = new List<string>() { "Administrador", "Cliente", "Empleado" };

            foreach (string rol in roles)
            {
                await CrearRole(rol);
            }
        }

        private async Task CrearRole(string rolName)
        {
            if (!await _rolManager.RoleExistsAsync(rolName))
            {
                await _rolManager.CreateAsync(new Rol(rolName));
            }
        }

        public async Task<IActionResult> CrearAdmin()
        {
            Persona administrador = new Persona()
            {
                Nombre = "Darth",
                Apellido = "Vader",
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                Telefono = new Telefono { CodigoDeArea = 15, Numero = 66666666 },
                Direccion = new Direccion { Localidad = "CABA", Calle = "Larrea", Numero = 256, CodigoPostal = 1238, Piso = 7 },
                Foto = "admin.jpg"
            };

            var resuAdm = await _userManager.CreateAsync(administrador, passDefault);
            if (resuAdm.Succeeded)
            {
                string rolAdm = "Administrador";
                await CrearRole(rolAdm);
                await _userManager.AddToRoleAsync(administrador, rolAdm);
     
                TempData["Mensaje"] = $"Empleado creado {administrador.Email} y {passDefault}";
            }

            return RedirectToAction("Index", "Personas");
        }

        public async Task CrearEmpleado()
        {

                      Empleado empleado = new Empleado()
                    {
                        Nombre = "StormTrooper",
                        Apellido = "N1",
                        Email = "empleado@empleado.com",
                        UserName = "empleado@empleado.com",
                        Telefono = new Telefono { CodigoDeArea = 15, Numero = 58965897 },
                        Direccion = new Direccion { Localidad = "CABA", Calle = "Av Corrientes", Numero = 5896, CodigoPostal = 1258, Piso = 2 },
                        Foto = "empleado.jpg"
                      };

                var resuEmpl = await _userManager.CreateAsync(empleado, passDefault);
                    if (resuEmpl.Succeeded)
                    {
                        string rolAdm = "Empleado";
                        await CrearRole(rolAdm);
                        await _userManager.AddToRoleAsync(empleado, rolAdm);

                        TempData["Mensaje"] = $"Empleado creado {empleado.Email} y {passDefault}";
                    }


        }

        public async Task CrearClientes()
        {
            if (_carritoContext.Clientes.Count() < 10)
            {
                int cantClientes = 10;
                int indexCliente = _carritoContext.Clientes.Count() + 1;

                for (int i = indexCliente; i <= cantClientes; i++)
                {
                    Cliente clt = new Cliente()
                    {

                        Email = "cliente" + i.ToString() + "@ort.com",
                        UserName = "cliente" + i.ToString() + "@ort.com",
                        Apellido = "Cliente",
                        Nombre = ("Cliente " + i).ToString(),
                        Dni = 1122233 + i,
                        Telefono = new Telefono { CodigoDeArea = 15, Numero = 2222333 + i },
                        Direccion = new Direccion { Localidad = "CABA", Calle = "9 de julio", Numero = 4586 + i, CodigoPostal = 1234, Piso = 1 },
                        Foto = Configuraciones.FotosClientes[i - 1]


                    };
                    await _carritoContext.SaveChangesAsync();

                    

                    var resultadoCreacion = await _userManager.CreateAsync(clt, Configuraciones.PassPorDefecto);

                    if (resultadoCreacion.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(clt, "Cliente");
                    }
                    Carrito carrito = new Carrito();
                    carrito.ClienteId = clt.Id;
                    _carritoContext.Carritos.Add(carrito);
                    await _carritoContext.SaveChangesAsync();
                }
            }
        }

        public async Task CrearCategorias()
        {
            Categoria cat1 = new Categoria() { Nombre = "Camperas", Descripcion = "Abrigos" };
            Categoria cat2 = new Categoria() { Nombre = "Zapatos", Descripcion = "Zapatos" };
            Categoria cat3 = new Categoria() { Nombre = "Pantalones", Descripcion = "Pantalones" };

            _carritoContext.Categorias.Add(cat1);
            _carritoContext.Categorias.Add(cat2);
            _carritoContext.Categorias.Add(cat3);
            await _carritoContext.SaveChangesAsync();

        }

        public async Task CrearProductos()
        {

            Categoria cat_campera = _carritoContext.Categorias.Where(c => c.Nombre == "Camperas").First();
            Categoria cat_zapatos = _carritoContext.Categorias.Where(c => c.Nombre == "Zapatos").First();
            Categoria cat_pantalones = _carritoContext.Categorias.Where(c => c.Nombre == "Pantalones").First();
            
            Producto prod1 = new Producto() { Nombre = "Campera Freaky", Descripcion = "De algodon, color negro, con cierres metalicos", PrecioVigente = 7000, Activo = true, CategoriaId = cat_campera.Id, Foto= "camperafreaky.jpg" };
            Producto prod2 = new Producto() { Nombre = "Mocasines Cuero", Descripcion = "De cuero, uso cotidiano, color negro y marrón", PrecioVigente = 2860, Activo = true, CategoriaId = cat_zapatos.Id, Foto = "mocasinescuero.jpg" };
            Producto prod3 = new Producto() { Nombre = "Pantalón Jean Azul", Descripcion = "Jean tiro recto, color azul", PrecioVigente = 5400, Activo = true, CategoriaId = cat_pantalones.Id, Foto = "jeanazul.jpg" };
            Producto prod4 = new Producto() { Nombre = "Campera Nike", Descripcion = "unisex, color gris oscuro, con capucha", PrecioVigente = 10000, Activo = false, CategoriaId = cat_campera.Id, Foto = "camperanike.jpg" };
            Producto prod5 = new Producto() { Nombre = "Pantalón Gabardina", Descripcion = "color verde militar, con bolsillos", PrecioVigente = 4700, Activo = true, CategoriaId = cat_pantalones.Id, Foto = "pantalongabardina.jpg" };
            Producto prod6 = new Producto() { Nombre = "Campera Adidas", Descripcion = "unisex, color amarillo y azul, sin capucha", PrecioVigente = 15000, Activo = true, CategoriaId = cat_campera.Id, Foto = "camperaadidas.jpg" };
            Producto prod7 = new Producto() { Nombre = "Pantalón Jean Negro", Descripcion = "Jean tiro recto, color negro", PrecioVigente = 5400, Activo = true, CategoriaId = cat_pantalones.Id, Foto = "jeannegro.jpg" };
            Producto prod8 = new Producto() { Nombre = "Mocasines Gamuza", Descripcion = "color azul, uso cotidiano", PrecioVigente = 2530, Activo = true, CategoriaId = cat_zapatos.Id, Foto = "mocasinesgamuza.jpg" };

             _carritoContext.Productos.Add(prod1);
            _carritoContext.Productos.Add(prod2);
            _carritoContext.Productos.Add(prod3);
            _carritoContext.Productos.Add(prod4);
            _carritoContext.Productos.Add(prod5);
            _carritoContext.Productos.Add(prod6);
            _carritoContext.Productos.Add(prod7);
            _carritoContext.Productos.Add(prod8);



            await _carritoContext.SaveChangesAsync();
        }

        public async Task CrearSucursales()
        {
            Sucursal suc1 = new Sucursal() { Nombre = "CABA", Direccion = "Corrientes 2311", Telefono = "47574545", Email ="corrientes@sucursal.com" };
            Sucursal suc2 = new Sucursal() { Nombre = "Palermo", Direccion = "Av. Santa Fe 4300", Telefono = "47579999", Email ="santafe@sucursal.com" };

            _carritoContext.Sucursales.Add(suc1);
            _carritoContext.Sucursales.Add(suc2);
            await _carritoContext.SaveChangesAsync();
        }

        public async Task CrearStock()
        {
            StockItem stock1 = new StockItem() { ProductoId = 1, SucursalId = 1, Cantidad= 10};
            StockItem stock2 = new StockItem() { ProductoId = 1, SucursalId = 2, Cantidad = 10 };
            StockItem stock3 = new StockItem() { ProductoId = 2, SucursalId = 1, Cantidad = 5 };
            StockItem stock4 = new StockItem() { ProductoId = 2, SucursalId = 2, Cantidad = 15 };
            StockItem stock5 = new StockItem() { ProductoId = 3, SucursalId = 1, Cantidad = 20 };
            StockItem stock6 = new StockItem() { ProductoId = 3, SucursalId = 2, Cantidad = 20 };
            StockItem stock7 = new StockItem() { ProductoId = 6, SucursalId = 1, Cantidad = 20 };
            StockItem stock8 = new StockItem() { ProductoId = 5, SucursalId = 2, Cantidad = 10 };

            _carritoContext.StockItems.Add(stock1);
            _carritoContext.StockItems.Add(stock2);
            _carritoContext.StockItems.Add(stock3);
            _carritoContext.StockItems.Add(stock4);
            _carritoContext.StockItems.Add(stock5);
            _carritoContext.StockItems.Add(stock6);
            _carritoContext.StockItems.Add(stock7);
            _carritoContext.StockItems.Add(stock8);
            await _carritoContext.SaveChangesAsync();
        }


    }
}
