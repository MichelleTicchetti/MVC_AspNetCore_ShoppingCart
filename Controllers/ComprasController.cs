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

namespace Carrito_A.Controllers
{
    public class ComprasController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;

        public ComprasController(CarritoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        /*
         // GET: Compras
         public async Task<IActionResult> Index()
         {

             return View(await _context.Compras.ToListAsync());
         }
        
        */
        [Authorize(Roles = ("Cliente"))]
        public IActionResult VerComprasCliente()
        {

            var miCliente = _userManager.GetUserAsync(User);
            var clienteId = miCliente.Result.Id;

            var compras = _context.Compras
                .Where(c => c.ClienteId == clienteId)
                    .Include(c => c.Sucursal)
                    .Include(c => c.Carrito)
                    .ThenInclude(c => c.CarritoItems)
                    .ThenInclude(c => c.Producto)
                    .ToList();

            if (compras.Any())
                {
                return View("ListarMisCompras", compras);
                }
                else
                {
                    return RedirectToAction(nameof(ErrorNoTieneCompras));
                }

        }


        [Authorize(Roles = ("Empleado, Administrador"))]
        public IActionResult ListarComprasMesOrdenDescValor()
        {

            DateTime Hoy = DateTime.Today;
            int Mes = Hoy.Month;
            int Anio = Hoy.Year;

            var compras = _context.Compras
                  .Where(c => c.Fecha.Month == Mes && c.Fecha.Year == Anio)
                  .Include(c=>c.Cliente)
                  .Include(c=>c.Sucursal)
                  .Include(c => c.Carrito)
                  .ThenInclude(c => c.CarritoItems)
                  .ThenInclude(c => c.Producto)
                  .ToList()
                  .OrderByDescending(c => c.Total);

            return View(compras);

        }


        public IActionResult ErrorNoTieneCompras()
        {
            return View();
        }

        // GET: Compras/Details/5
        public IActionResult DetalleCompraCliente(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<CarritoItem> items = _context.CarritoItems
                 .Where(c => c.CarritoId == id)
                 .Include(c => c.Producto)
                 .ToList();



            return View(items);
        }

        /*// GET: Compras/Details/5
         public async Task<IActionResult> Details(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

            var compra = await _context.Compra
                .FirstOrDefaultAsync(m => m.Id == id);
             if (compra == null)
             {
                 return NotFound();
             }

            return View(compra);
         }
         */

        /*
        // GET: Compras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(compra);
        }
        */
        // GET: Compras/Edit/5

        /* No se puede editar ni eliminar una compra
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total")] Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.Id))
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
            return View(compra);
        }
        */
        /*
        //GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        */

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }

        public bool VerificarStockCarritoSucursal(Carrito carrito, Sucursal sucursal)
        {
            bool hayStock = true;
            var i = 0;
            if(carrito != null && sucursal != null)
            {
                while(hayStock == true && i < carrito.CarritoItems.Count())
                {
                    var productoId = carrito.CarritoItems[i].ProductoId;
                    var cantidadProd = carrito.CarritoItems[i].Cantidad;
                    hayStock = VerificarStockProductoSucursal(productoId, cantidadProd, sucursal);
                    i++;
                }
            }
            else
            {
                hayStock = false;
            }

            return hayStock;
        }

        private bool VerificarStockProductoSucursal(int productoId, int cantidadProd, Sucursal sucursal)
        {
            bool hayStock = false;
            StockItem item = sucursal.StockItems.FirstOrDefault(i => i.ProductoId == productoId);

            if(item!= null)
            {
                var stock = item.Cantidad;

                if(stock >= cantidadProd)
                {
                    hayStock = true;
                }
            }

            return hayStock;
        }

        [Authorize(Roles = ("Cliente"))]
        public ViewResult FinalizarCompra(int carritoId, int sucursalId)
        {

            Carrito carrito = _context.Carritos
            .Where(c => c.Id == carritoId)
            .Include(c => c.Cliente)
            .Include(c => c.CarritoItems)
            .ThenInclude(c => c.Producto)
            .FirstOrDefault();

            if (carrito.CarritoItems.Any())
            {

                //1. tengo que pedirle a la vista que me pase una sucursal elegida por el cliente

                Sucursal sucursal = _context.Sucursales
                    .Where(s => s.Id == sucursalId)
                    .Include(s => s.StockItems)
                    .FirstOrDefault();

                Compra miCompra = null;

                //2. tengo que verificar si hay stock en cualquiera de las sucursales

                List<Sucursal> sucursalesTienenStock = this.SucursalesTienenStock(carrito);

                if (sucursalesTienenStock.Count() == 0){

                    return View("SinStock");

                }else{

                    //3. tengo que verificar si hay stock en la sucursal seleccionada
                    if (this.VerificarStockCarritoSucursal(carrito, sucursal))
                    {
                        //si sale todo bien creo la compra pasando el carrito y la sucursal
                        miCompra = this.CrearCompra(carrito, sucursal);
                        

                    }else{   
                        //4.tengo que ofrecer al cliente que seleccione otra sucursal con stock
                           ViewBag.ListaSucursalesStock = sucursalesTienenStock;
                            return View("ElegirSucursalStock", carrito);
                    }

                    ViewBag.Carrito = carrito;
                    ViewBag.Compra = miCompra;
                    ViewBag.Sucursal = sucursal;
                    return View("FinalizarCompra", miCompra.Carrito.CarritoItems);

                }
            }
            else
            {
                return View("ErrorCarritoSinItems");
            }
        }

    public List<Sucursal> SucursalesTienenStock(Carrito carrito)
        {

            var sucursales = _context.Sucursales.Include(s => s.StockItems).ToList();

            List<Sucursal> sucursalesConStock = new List<Sucursal>();

            foreach (Sucursal suc in sucursales)
            {
                if (this.VerificarStockCarritoSucursal(carrito, suc))
                {
                    sucursalesConStock.Add(suc);
                }
            }

            return sucursalesConStock;

        }

        [Authorize(Roles = ("Cliente"))]
        private Compra CrearCompra(Carrito carrito, Sucursal sucursal)
        {
            //paso el carrito a no activo, de esta manera el carrito se desactiva automáticamente al finalizar la compra
            carrito.Activo = false;
            _context.Carritos.Update(carrito);

            //tengo que descontar el stock de todos los items de la sucursal de mi compra
            foreach (CarritoItem carritoItem in carrito.CarritoItems)
            {
                StockItem i = sucursal.StockItems.FirstOrDefault(s => s.ProductoId == carritoItem.ProductoId);
                i.Cantidad = i.Cantidad - carritoItem.Cantidad;
                _context.StockItems.Update(i);
                _context.SaveChanges();

            }

            //creo una nueva compra para el cliente
            var cliente = carrito.Cliente;

            Compra compra = new Compra()
            {
                Cliente = cliente,
                ClienteId = cliente.Id,
                Carrito = carrito,
                CarritoId = carrito.Id,
                Fecha = DateTime.Now,
                Total = carrito.SubTotal,
                Sucursal = sucursal,
                SucursalId = sucursal.Id
            };
            _context.Compras.Add(compra);
            _context.SaveChanges();


            //creo un nuevo carrito para el cliente en estado activo
            Carrito newCarrito = new Carrito();
            newCarrito.ClienteId = cliente.Id;
            _context.Carritos.Add(newCarrito);
            _context.SaveChanges();



            return compra;
        }
    }

   
}
