using Carrito_A.Data;
using Carrito_A.Models;
using Carrito_A.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.Controllers
{
    public class AccountController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signinManager;
        //sacar desp para pasar a preCarga
        private readonly RoleManager<Rol> _rolManager;
        private const string passDefault = "Password1!";

        public AccountController(
            CarritoContext context,
            UserManager<Persona> userManager,
            SignInManager<Persona> signinManager,
            RoleManager<Rol> rolManager

        )

        {
            this._context = context;
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._rolManager = rolManager;
        }

        [HttpGet]
        public IActionResult EmailDisponible(string email)
        {
            var emailUsado = _context.Personas.Any(p => p.Email == email);

            if (!emailUsado)
            {
                //el email está disponible               
                return Json(true);
            }
            else
            {
                return Json($"El correo {email} ya está en uso.");
            }
        }


        public ActionResult Registrar()
        {
            return View();
        }


        //El cliente puede autoregistrarse
        [HttpPost]
        public async Task<ActionResult> Registrar(RegistracionViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                Cliente cliente = new Cliente();
                cliente.UserName = viewModel.Email;
                cliente.Email = viewModel.Email;
                cliente.Nombre = viewModel.Nombre;
                cliente.Apellido = viewModel.Apellido;
                cliente.Dni = viewModel.Dni;
                            


                var resultadoCreacion = await _userManager.CreateAsync(cliente, viewModel.Password);

                if (resultadoCreacion.Succeeded)
                {

                    //crear roles en la preCarga
                    await CrearRolesBase();

                    //agrego a un usuario un rol específico, le paso el cliente y el rol que le quiero agregar
                    var resultado = await _userManager.AddToRoleAsync(cliente, "Cliente");

                    if (resultado.Succeeded)
                    {
                        //direccion
                        Direccion direccion = new Direccion();
                        direccion.PersonaId = cliente.Id;
                        direccion.Localidad = viewModel.Localidad;
                        direccion.Calle = viewModel.Calle;
                        direccion.Numero = viewModel.Numero;
                        direccion.CodigoPostal = viewModel.CodigoPostal;
                        direccion.Piso = viewModel.Piso.Value;
                        direccion.Departamento = viewModel.Departamento;
                        _context.Direcciones.Add(direccion);

                        //le creo el telefono
                        Telefono telefono = new Telefono();
                        telefono.PersonaId = cliente.Id;
                        telefono.CodigoDeArea = viewModel.CodigoDeArea;
                        telefono.Numero = viewModel.NumeroTelefono;
                        _context.Telefonos.Add(telefono);

                        //compras?

                        //al registrar un cliente le asigno un carrito: El carrito se crea automaticamente con la creación de un cliente, en estado activo.

                        Carrito carrito = new Carrito();
                        carrito.ClienteId = cliente.Id;
                        _context.Carritos.Add(carrito);
                        _context.SaveChanges();
                        

                        //hago el proceso de sign-in con el usuario, una vez que el usuario se registró ya queda logueado en el sistema
                        await _signinManager.SignInAsync(cliente, isPersistent: false);

                        return RedirectToAction("Index", "Productos");
                    }
                }

                //tratamiento de errores
                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(viewModel);
        }

        private async Task CrearRolesBase()
        {
            List<string> roles = new List<string>() { "Administrador", "Cliente", "Empleado"};

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

        [HttpGet]
        public ActionResult IniciarSesion(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;

            if (returnUrl != null)
            {
                ViewBag.Mensaje = "Para acceder al recurso " + returnUrl + ", primero debe Iniciar sesión";

            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IniciarSesion(Login viewModel)
        {
            string returnUrl = TempData["returnUrl"] as string;

            if (ModelState.IsValid)
            {
                var resultadoSignIn = await _signinManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.Recordarme, false);

                if (resultadoSignIn.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    if (User.IsInRole("Cliente")) { return RedirectToAction("CheckIn", "Clientes"); }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sesión inválido");
                }

            }

            return View(viewModel);
        }

        public async Task<ActionResult> CerrarSesion()
        {
            await _signinManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        public async Task<IActionResult> CrearAdmin()
        {
            Persona administrador = new Persona()
            {
                Nombre = "Admin",
                Apellido = "Admin",
                Email = "admin@admin.com",
                UserName = "admin@admin.com"
            };

            var resuAdm = await _userManager.CreateAsync(administrador, passDefault);
            if (resuAdm.Succeeded)
            {
                string rolAdm = "Administrador";
                await CrearRole(rolAdm);
                await _userManager.AddToRoleAsync(administrador, rolAdm);
                await _signinManager.SignInAsync(administrador, isPersistent: false);
                TempData["Mensaje"] = $"Empleado creado {administrador.Email} y {passDefault}";
            }

            return RedirectToAction("Index", "Personas");
        }

        public async Task<IActionResult> CrearEmpleado()
        {
            Empleado empleado = new Empleado()
            {
                Nombre = "Empleado",
                Apellido = "Empleado",
                Email = "empleado@empleado.com",
                UserName = "empleado@empleado.com"
            };

            var resuAdm = await _userManager.CreateAsync(empleado, passDefault);
            if (resuAdm.Succeeded)
            {
                string rolAdm = "Empleado";
                await CrearRole(rolAdm);
                await _userManager.AddToRoleAsync(empleado, rolAdm);
                await _signinManager.SignInAsync(empleado, isPersistent: false);
                TempData["Mensaje"] = $"Empleado creado {empleado.Email} y {passDefault}";
            }

            
            return RedirectToAction("Index", "Personas");
        }

        
    }

}