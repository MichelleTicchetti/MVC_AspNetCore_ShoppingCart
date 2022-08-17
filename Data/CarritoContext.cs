using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carrito_A.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Carrito_A.Data
{
    public class CarritoContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public CarritoContext(DbContextOptions configuracion) : base(configuracion)
        {

        }

        public static readonly ILoggerFactory MiLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLoggerFactory(MiLoggerFactory);


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            //Definición de nombre de tablas
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");


        }

        public DbSet<Carrito_A.Models.Rol> Roles { get; set; }

        public DbSet<Carrito_A.Models.Persona> Personas { get; set; }

        public DbSet<Carrito_A.Models.Carrito> Carritos { get; set; }

        public DbSet<Carrito_A.Models.CarritoItem> CarritoItems { get; set; }

        public DbSet<Carrito_A.Models.Producto> Productos { get; set; }

        public DbSet<Carrito_A.Models.Cliente> Clientes { get; set; }

        public DbSet<Carrito_A.Models.Empleado> Empleados { get; set; }

        public DbSet<Carrito_A.Models.Categoria> Categorias { get; set; }

        public DbSet<Carrito_A.Models.Compra> Compras { get; set; }

        public DbSet<Carrito_A.Models.StockItem> StockItems { get; set; }

        public DbSet<Carrito_A.Models.Sucursal> Sucursales { get; set; }

        public DbSet<Carrito_A.Models.Direccion> Direcciones { get; set; }

        public DbSet<Carrito_A.Models.Telefono> Telefonos { get; set; }

    }
}
