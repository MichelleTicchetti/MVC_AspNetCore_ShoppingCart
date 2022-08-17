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

namespace Carrito_A.Controllers
{
    public class StockItemsController : Controller
    {
        private readonly CarritoContext _context;

        public StockItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: StockItems
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.StockItems.Include(s => s.Producto).Include(s => s.Sucursal);
            return View(await carritoContext.ToListAsync());
        }

        // GET: StockItems/Details/5
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItems
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // GET: StockItems/Create
        [Authorize(Roles = ("Empleado, Administrador"))]
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            ViewData["SucursalId"] = new SelectList(_context.Set<Sucursal>(), "Id", "Nombre");
            return View();
        }

        // POST: StockItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Create([Bind("Id,SucursalId,ProductoId,Cantidad")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Set<Sucursal>(), "Id", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StockItems/Edit/5
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItems.FindAsync(id);
            if (stockItem == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Set<Sucursal>(), "Id", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }

        // POST: StockItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SucursalId,ProductoId,Cantidad")] StockItem stockItem)
        {
            if (id != stockItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockItemExists(stockItem.Id))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Set<Sucursal>(), "Id", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }

        /* no se pueden eliminar stock items desde el sistema, se eliminan junto con la sucursal cuando esta sea eliminada
        // GET: StockItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // POST: StockItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockItem = await _context.StockItem.FindAsync(id);
            _context.StockItem.Remove(stockItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        */
        private bool StockItemExists(int id)
        {
            return _context.StockItems.Any(e => e.Id == id);
        }
    }
}
