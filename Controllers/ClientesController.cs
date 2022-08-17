using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_A.Data;
using Carrito_A.Models;
using Carrito_A.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Carrito_A.Helpers;

namespace Carrito_A.Controllers
{
    public class ClientesController : Controller
    {
        private readonly CarritoContext _context;
        private readonly IWebHostEnvironment _hosting;

        public ClientesController(CarritoContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            this._hosting = hosting;
        }

        // GET: Clientes
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Direccion)
                .Include(c => c.Telefono)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }


        [Authorize(Roles = ("Empleado, Administrador"))]
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Create([Bind("Dni,Id,Nombre,Apellido,Email,UserName,FechaAlta,Password")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }


        /*// GET: Clientes/Edit/5
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int id, [Bind("Dni,Id,Nombre,Apellido,Email,UserName,FechaAlta,Password")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        */

        [Authorize(Roles = ("Cliente"))]
        public async Task<IActionResult> MisDatos(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(clt => clt.Direccion)
                .Include(clt => clt.Telefono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public ActionResult EditarDireccionTelefono()
        {
            return View();
        }


        //El cliente puede autoregistrarse
        [HttpPost]
        public async Task<ActionResult> EditarDireccionTelefono(int clienteId, DireccionTelefono viewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Cliente cltEnDb = _context.Clientes.Find(clienteId);

                    var clt = _context.Clientes
                        .Where(clt => clt.Id == cltEnDb.Id)
                        .Include(clt => clt.Direccion)
                        .Include(clt => clt.Telefono)
                        .First();

                    clt.Direccion.Localidad = viewModel.Localidad;
                    clt.Direccion.Calle = viewModel.Calle;
                    clt.Direccion.Numero = viewModel.Numero;
                    clt.Direccion.Piso = viewModel.Piso;
                    clt.Direccion.Departamento = viewModel.Departamento;
                    clt.Direccion.CodigoPostal = viewModel.CodigoPostal;
                    clt.Telefono.CodigoDeArea = viewModel.CodigoDeArea;
                    clt.Telefono.Numero = viewModel.NumeroTelefono;

                    _context.Update(clt);
                    await _context.SaveChangesAsync();
                    return View("MisDatos", clt);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(clienteId))
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

        [Authorize(Roles = ("Empleado, Administrador"))]
        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize(Roles = ("Empleado, Administrador"))]
        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }


        [Authorize(Roles = ("Empleado, Administrador"))]
        public ActionResult EditarDatosCliente(int clienteId)
        {

            Cliente cliente = _context.Clientes
           .Where(e => e.Id == clienteId)
           .FirstOrDefault();

            ViewBag.Cliente = cliente;

            return View();
        }


        [Authorize(Roles = ("Empleado, Administrador"))]
        [HttpPost]
        public async Task<ActionResult> EditarDatosCliente(int clienteId, EditarDatosCliente viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Cliente clienteDb = _context.Clientes.Find(clienteId);

                    var c = _context.Clientes
                        .Where(c => c.Id == clienteDb.Id)
                        .Include(c => c.Direccion)
                        .Include(c => c.Telefono)
                        .First();

                    c.Nombre = viewModel.Nombre;
                    c.Apellido = viewModel.Apellido;
                    c.Direccion.Localidad = viewModel.Localidad;
                    c.Direccion.Calle = viewModel.Calle;
                    c.Direccion.Numero = viewModel.Numero;
                    c.Direccion.Piso = viewModel.Piso;
                    c.Direccion.Departamento = viewModel.Departamento;
                    c.Direccion.CodigoPostal = viewModel.CodigoPostal;
                    c.Telefono.CodigoDeArea = viewModel.CodigoDeArea;
                    c.Telefono.Numero = viewModel.NumeroTelefono;

                    _context.Update(c);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(clienteId))
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
        if (_context.Clientes.Any(c => c.Id == id))
        {
            TempData["ClienteId"] = id;
        }

        return View(new Representacion());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SubirFoto(Representacion modelo)
    {

        if (ModelState.IsValid)
        {

            int clienteId = (int)TempData["ClienteId"];

            if (clienteId != 0)
            {
                try
                {
                    Cliente cltInDb = _context.Clientes.Find(clienteId);

                    if (cltInDb == null)
                    {
                        return NotFound();
                    }

                    if (modelo.Imagen != null)
                    {
                            cltInDb.Foto = Imagenes.Subir(_hosting.WebRootPath, Configuraciones.ClientesPATH, string.Empty, modelo.Imagen);

                        if (!string.IsNullOrEmpty(cltInDb.Foto))
                        {
                            _context.Personas.Update(cltInDb);
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

        int clienteId = (int)TempData["ClienteId"];

        if (clienteId == null)
        {
            return NotFound();
        }
        else
        {
            Cliente cliente = _context.Clientes.Find(clienteId);

            if (cliente != null)
            {
                if (cliente.Foto != null)
                {
                    string nuevaFoto = Configuraciones.PersonaDef;
                        cliente.Foto = nuevaFoto;
                    _context.Personas.Update(cliente);
                    _context.SaveChanges();

                }
            }
        }




        return RedirectToAction("Index", "Home");
    }


}
}
