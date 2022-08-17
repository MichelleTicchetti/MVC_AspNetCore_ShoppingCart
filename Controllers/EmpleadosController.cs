using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_A.Data;
using Carrito_A.Models;
using Microsoft.AspNetCore.Authorization;
using Carrito_A.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Carrito_A.Helpers;

namespace Carrito_A.Controllers
{
    [Authorize(Roles = ("Empleado, Administrador"))]
    public class EmpleadosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly IWebHostEnvironment _hosting;

        public EmpleadosController(CarritoContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            this._hosting = hosting;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleados.ToListAsync());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(c => c.Direccion)
                .Include(c => c.Telefono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Legajo,Id,Nombre,Apellido,Email,UserName,FechaAlta,Password")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Legajo,Id,Nombre,Apellido,Email,UserName,FechaAlta,Password")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }


        [Authorize(Roles = ("Administrador"))]
        public ActionResult EditarDatosEmpleado(int empleadoId)
        {

            Empleado empleado = _context.Empleados
           .Where(e => e.Id == empleadoId)
           .FirstOrDefault();

            ViewBag.Empleado = empleado;

            return View();
        }


        [Authorize(Roles = ("Administrador"))]
        [HttpPost]
        public async Task<ActionResult> EditarDatosEmpleado(int empleadoId, EditarDatosEmpleado viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Empleado empladoDb = _context.Empleados.Find(empleadoId);

                    var e = _context.Empleados
                        .Where(e => e.Id == empladoDb.Id)
                        .Include(e => e.Direccion)
                        .Include(e => e.Telefono)
                        .First();

                    e.Nombre = viewModel.Nombre;
                    e.Apellido = viewModel.Apellido;
                    e.Direccion.Localidad = viewModel.Localidad;
                    e.Direccion.Calle = viewModel.Calle;
                    e.Direccion.Numero = viewModel.Numero;
                    e.Direccion.Piso = viewModel.Piso;
                    e.Direccion.Departamento = viewModel.Departamento;
                    e.Direccion.CodigoPostal = viewModel.CodigoPostal;
                    e.Telefono.CodigoDeArea = viewModel.CodigoDeArea;
                    e.Telefono.Numero = viewModel.NumeroTelefono;

                    _context.Update(e);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleadoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }

            }

            return View(viewModel);
        }


        [HttpGet]
        [Authorize]
        public IActionResult SubirFoto(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            if (_context.Empleados.Any(e => e.Id == id))
            {
                TempData["EmpleadoId"] = id;
            }

            return View(new Representacion());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubirFoto(Representacion modelo)
        {

            if (ModelState.IsValid)
            {

                int empleadoId = (int)TempData["EmpleadoId"];

                if (empleadoId != 0)
                {
                    try
                    {
                        Empleado empleInDb = _context.Empleados.Find(empleadoId);

                        if (empleInDb == null)
                        {
                            return NotFound();
                        }

                        if (modelo.Imagen != null)
                        {
                            empleInDb.Foto = Imagenes.Subir(_hosting.WebRootPath, Configuraciones.EmpleadosPATH, string.Empty, modelo.Imagen);

                            if (!string.IsNullOrEmpty(empleInDb.Foto))
                            {
                                _context.Empleados.Update(empleInDb);
                                _context.SaveChanges();
                                return RedirectToAction("Index", "Home");
                            }

                            ModelState.AddModelError(string.Empty, "Error al cargar la imagen");
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return View(modelo);
        }

        //Eliminación Lógica, no quiero eliminarla fisicamente aquí.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EliminarFoto()
        {

            int empleadoId = (int)TempData["EmpleadoId"];

            if (empleadoId == null)
            {
                return NotFound();
            }
            else
            {
                Empleado empleado = _context.Empleados.Find(empleadoId);

                if (empleado != null)
                {
                    if (empleado.Foto != null)
                    {
                        string nuevaFoto = Configuraciones.PersonaDef;
                        empleado.Foto = nuevaFoto;
                        _context.Personas.Update(empleado);
                        _context.SaveChanges();

                    }
                }
            }




            return RedirectToAction("Index", "Home");
        }

    }
}
