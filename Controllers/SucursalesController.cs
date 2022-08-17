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
using Microsoft.Data.SqlClient;

namespace Carrito_A.Controllers
{
    public class SucursalesController : Controller
    {
        private readonly CarritoContext _context;

        public SucursalesController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Sucursales
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.Sucursales;
            return View(await carritoContext.ToListAsync());
        }

        // GET: Sucursales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // GET: Sucursales/Create
        [Authorize(Roles = ("Empleado, Administrador"))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sucursales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sucursal);

                try
                { 
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException excepcion)
                {
                    SqlException innerException = excepcion.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError(string.Empty, "Nombre de sucursal existente.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, excepcion.Message);
                    }

                }                                
            }
            return View(sucursal);
        }

        // GET: Sucursales/Edit/5
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }
            return View(sucursal);
        }

        // POST: Sucursales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Direccion,Email,Telefono")] Sucursal sucursal)
        {
            if (id != sucursal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sucursal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SucursalExists(sucursal.Id))
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
            return View(sucursal);
        }

        // GET: Sucursales/Delete/5
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales
                .Include(s => s.StockItems)
                .Include(s => s.Compras)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // POST: Sucursales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);

            //antes de eliminar sucursal debo eliminar stock
            Sucursal suc = _context.Sucursales
                            .Include(s => s.StockItems)
                            .Include(s => s.Compras)
                            .Where(s => s.Id == id).First();
            
            foreach (var item in suc.StockItems)
            {
                _context.StockItems.Remove(item);
            }


            _context.Sucursales.Remove(sucursal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SucursalExists(int id)
        {
            return _context.Sucursales.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ElegirSucursal(int carritoId)
        {
            var sucursales = _context.Sucursales.ToList();

            Carrito carrito = _context.Carritos
           .Where(c => c.Id == carritoId)
           .Include(c => c.Cliente)
           .Include(c => c.CarritoItems)
           .ThenInclude(c => c.Producto)
           .FirstOrDefault();

            ViewBag.Carrito = carrito;

            return View(sucursales);
        }

    }
}
