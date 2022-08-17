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
    public class DireccionesController : Controller
    {
        private readonly CarritoContext _context;

        public DireccionesController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Direcciones
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.Direcciones.Include(d => d.Persona);
            return View(await carritoContext.ToListAsync());
        }

        // GET: Direcciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // GET: Direcciones/Create
        public IActionResult Create()
        {
            var personas = _context.Personas
                .Include(p => p.Direccion)
                .Where(p => p.Direccion == null)
                .ToList();

            ViewData["PersonaId"] = new SelectList(personas, "Id", "NombreCompleto");
            return View();
        }

        // POST: Direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Localidad,Calle,Numero,Piso,Departamento,CodigoPostal,PersonaId")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "NombreCompleto", direccion.PersonaId);
            return View(direccion);
        }

        /*
        //Create para sucursales
        [Authorize(Roles = ("Empleado, Administrador"))]
        public IActionResult CreateDireccionSucursal()
        {
            var sucursales = _context.Sucursales
                .Include(s => s.Direccion)
                .Where(s => s.Direccion == null)
                .ToList();

            ViewData["SucursalId"] = new SelectList(sucursales, "Id", "Nombre");
            return View();
        }

        [Authorize(Roles = ("Empleado, Administrador"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDireccionSucursal([Bind("Id,Localidad,Calle,Numero,Piso,Departamento,CodigoPostal,SucursalId")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                _context.Direcciones.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Categorias");
            }
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Nombre", direccion.SucursalId);

            return View(direccion);
        }

        */

        // GET: Direcciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion == null)
            {
                return NotFound();
            }
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "NombreCompleto", direccion.PersonaId);
            return View(direccion);
        }

        // POST: Direcciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Localidad,Calle,Numero,Piso,Departamento,CodigoPostal,PersonaId")] Direccion direccion)
        {
            if (id != direccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.Id))
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
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "NombreCompleto", direccion.PersonaId);
            return View(direccion);
        }

        // GET: Direcciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // POST: Direcciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direccion = await _context.Direcciones.FindAsync(id);
            _context.Direcciones.Remove(direccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DireccionExists(int id)
        {
            return _context.Direcciones.Any(e => e.Id == id);
        }
    }
}
