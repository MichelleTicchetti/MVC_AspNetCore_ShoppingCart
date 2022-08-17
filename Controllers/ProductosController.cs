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
using Carrito_A.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace Carrito_A.Controllers
{
    public class ProductosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly IWebHostEnvironment _hosting;

        public ProductosController(CarritoContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            this._hosting = hosting;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.Productos.Include(p => p.Categoria);
            return View(await carritoContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create

        [Authorize(Roles = ("Empleado, Administrador"))]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,PrecioVigente,Activo,CategoriaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);

    
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,PrecioVigente,Activo,CategoriaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }



        /*No pueden eliminarse del sistema.
         Solo los producto pueden dehabilitarse.

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        */

        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> HabilitarProducto(int? id)
        {
            var producto = _context.Productos.Find(id);
            producto.Activo = true;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = ("Empleado, Administrador"))]
        public async Task<IActionResult> DeshabilitarProducto(int? id)
        {
            var producto = _context.Productos.Find(id);
            producto.Activo = false;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }


        [HttpGet]
        [Authorize]
        public IActionResult SubirFoto(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            if (_context.Productos.Any(p => p.Id == id))
            {
                TempData["ProductoId"] = id;
            }

            return View(new Representacion());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubirFoto(Representacion modelo)
        {

            if (ModelState.IsValid)
            {

                int productoId = (int)TempData["ProductoId"];

                if (productoId != 0)
                {
                    try
                    {
                        Producto prodInDb = _context.Productos.Find(productoId);

                        if (prodInDb == null)
                        {
                            return NotFound();
                        }

                        if (modelo.Imagen != null)
                        {
                            prodInDb.Foto = Imagenes.Subir(_hosting.WebRootPath, Configuraciones.ProductosPATH, string.Empty, modelo.Imagen);

                            if (!string.IsNullOrEmpty(prodInDb.Foto))
                            {
                                _context.Productos.Update(prodInDb);
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

            int productoId = (int)TempData["ProductoId"];

            if (productoId == null)
            {
                return NotFound();
            }
            else
            {
                Producto producto = _context.Productos.Find(productoId);

                if (producto != null)
                {
                    if (producto.Foto != null)
                    {
                        string nuevaFoto = Configuraciones.ProductoDef;
                        producto.Foto = nuevaFoto;
                        _context.Productos.Update(producto);
                        _context.SaveChanges();

                    }
                }
            }
                              
           

            
            return RedirectToAction("Index", "Home");
        }


    }
}
