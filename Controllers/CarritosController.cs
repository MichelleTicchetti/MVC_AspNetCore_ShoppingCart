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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Carrito_A.Controllers
{
    public class CarritosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;

        public CarritosController(CarritoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            this._userManager = userManager;

        }

        [Authorize(Roles = ("Empleado, Administrador"))]
        // GET: Carritos
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.Carritos.Include(c => c.Cliente);
            return View(await carritoContext.ToListAsync());
        }

        [Authorize(Roles = ("Empleado, Administrador"))]
        // GET: Carritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        /*
        // GET: Carritos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "NombreCompleto");
            return View();
        }

        // POST: Carritos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Activo,ClienteId")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {

                _context.Add(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Apellido", carrito.ClienteId);
            return View(carrito);

        }

        */

        [Authorize(Roles = ("Empleado, Administrador"))]
        // GET: Carritos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos.FindAsync(id);


            if (carrito == null)
            {
                return NotFound();
            }


            {
                ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Apellido", carrito.ClienteId);
            }


            return View(carrito);
        }

        // POST: Carritos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activo,ClienteId")] Carrito carrito)
        {
            if (id != carrito.Id)
            {
                return NotFound();
            }

            if (carrito.Activo)
            {
                return RedirectToAction(nameof(ErrorEdicionCarrito));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        public IActionResult ErrorEdicionCarrito()
        {
            return View();
        }

        public IActionResult ErrorCarritoNoActivo()
        {
            return View();
        }

        public IActionResult ErrorProductoNoActivo()
        {
            return View();
        }

        /* No se puede eliminar carritos.
    // GET: Carritos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carrito = await _context.Carrito
            .Include(c => c.Cliente)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (carrito == null)
        {
            return NotFound();
        }

        return View(carrito);
    }

    // POST: Carritos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var carrito = await _context.Carrito.FindAsync(id);
        _context.Carrito.Remove(carrito);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

            */

        [Authorize(Roles = ("Cliente"))]
        public async Task<IActionResult> CarritoMgrAsync(int productoId)
        {
            var miCliente = await _userManager.GetUserAsync(User);
            var clienteId = miCliente.Id;

            Producto producto = _context.Productos.FirstOrDefault(p => p.Id == productoId);
            Cliente cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);
            Carrito carrito = _context.Carritos.FirstOrDefault(c => c.ClienteId == clienteId && c.Activo==true);

            if (cliente != null && producto != null && carrito != null && carrito.Activo == true && producto.Activo == true)
            {
                CarritoItem item = _context.CarritoItems
                    .FirstOrDefault(c => c.Carrito.ClienteId == clienteId && c.ProductoId == productoId && c.CarritoId == carrito.Id);

                //si ya existe ese item, le actualizo la cantidad
                if (item != null)
                {
                    item.Cantidad += item.Cantidad;
                    _context.CarritoItems.Update(item);
                    _context.Carritos.Update(carrito);
                    _context.SaveChanges();
                }

                //si no existe el item creo uno nuevo
                else
                {
                    CarritoItem nuevoItem = new CarritoItem()
                    {
                        ProductoId = productoId,
                        Producto = producto,
                        CarritoId = carrito.Id,
                        Carrito = carrito,
                        Cantidad = 1,
                        ValorUnitario = producto.PrecioVigente
                    };

                    _context.CarritoItems.Add(nuevoItem);
                    _context.Carritos.Update(carrito);
                    _context.SaveChanges();
                }

                List<CarritoItem> items = _context.CarritoItems.Where(c => c.CarritoId == carrito.Id).Include(c => c.Producto).ToList();
                return View(items);

            }
            else if (carrito.Activo == false)
            {
                return RedirectToAction(nameof(ErrorCarritoNoActivo));
            }

            else if (producto.Activo == false)
            {
                return RedirectToAction(nameof(ErrorProductoNoActivo));
            }

            else
            {
                return RedirectToAction("Index", "Carritos");
            }

        }

        [Authorize(Roles = ("Cliente"))]
        public IActionResult VaciarCarrito()
        {
            var miCliente = _userManager.GetUserAsync(User);
            var clienteId = miCliente.Result.Id;

            Cliente cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);
            Carrito carrito = _context.Carritos.Where(ca => ca.ClienteId == clienteId && ca.Activo == true).Include(ca => ca.CarritoItems).FirstOrDefault();

            if (cliente != null && carrito != null)
            {
                var items = carrito.CarritoItems.ToList();

                foreach (CarritoItem item in items)
                {
                    _context.CarritoItems.Remove(item);
                    _context.SaveChanges();
                }

                _context.Carritos.Update(carrito);
                _context.SaveChanges();

            }

            return RedirectToAction("Index", "Productos");
        }

        [Authorize(Roles = ("Cliente"))]
        public IActionResult MostrarCarrito()
        {

            var miCliente = _userManager.GetUserAsync(User);
            var clienteId = miCliente.Result.Id;

            var carritoItems = _context.CarritoItems
                .Where(i => i.Carrito.ClienteId == clienteId && i.Carrito.Activo)
                .Include(i => i.Producto)
                .Include(i => i.Carrito)
                .ToList();

            return View("CarritoMgr", carritoItems);
        }

        [Authorize(Roles = ("Cliente"))]
        public IActionResult AdicionarCantidad(int carritoItemId)
        {

         var miCliente = _userManager.GetUserAsync(User);
         var clienteId = miCliente.Result.Id;

         CarritoItem item = _context.CarritoItems
        .Where(c => c.Id == carritoItemId && c.Carrito.ClienteId == clienteId)
        .Include(c => c.Carrito.CarritoItems)
        .Include(c => c.Producto)
        .FirstOrDefault();


                item.Cantidad++;
                _context.CarritoItems.Update(item);
                _context.SaveChanges();

                List<CarritoItem> items = _context.CarritoItems.Where(c => c.CarritoId == item.CarritoId).Include(c => c.Carrito).Include(c => c.Producto).ToList();

                return View("CarritoMgr", items);
            }


        [Authorize(Roles = ("Cliente"))]
        public IActionResult RestarCantidad(int carritoItemId)
        {

            var miCliente = _userManager.GetUserAsync(User);
            var clienteId = miCliente.Result.Id;

            CarritoItem item = _context.CarritoItems
           .Where(c => c.Id == carritoItemId && c.Carrito.ClienteId == clienteId)
           .Include(c => c.Carrito.CarritoItems)
           .Include(c => c.Producto)
           .FirstOrDefault();

            if (item.Cantidad == 1)
            {
                _context.CarritoItems.Remove(item);
                _context.SaveChanges();
            }
            else
            {
                item.Cantidad--;
                _context.CarritoItems.Update(item);
                _context.SaveChanges();

            }

            List<CarritoItem> items = _context.CarritoItems.Where(c => c.CarritoId == item.CarritoId).Include(c => c.Carrito).Include(c => c.Producto).ToList();

            return View("CarritoMgr", items);
        }


        private bool CarritoExists(int id)
        {
            return _context.Carritos.Any(e => e.Id == id);
        }

    }
}

