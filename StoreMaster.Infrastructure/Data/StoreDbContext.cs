using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;

namespace StoreMaster.Infrastructure.Data;

public class StoreDbContext : IdentityDbContext<ApplicationUser>
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
    }

    // Una DbSet por cada entidad = una tabla en la BD
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetallesVenta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Filtro global: nunca devolver registros eliminados
        modelBuilder.Entity<Producto>().HasQueryFilter(p => !p.Eliminado);
        modelBuilder.Entity<Categoria>().HasQueryFilter(c => !c.Eliminado);
        modelBuilder.Entity<Proveedor>().HasQueryFilter(p => !p.Eliminado);
        modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.Eliminado);
        modelBuilder.Entity<Venta>().HasQueryFilter(v => !v.Eliminado);
        modelBuilder.Entity<DetalleVenta>().HasQueryFilter(d => !d.Eliminado);

        // Precisión de decimales para columnas de dinero
        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Producto>()
            .Property(p => p.PrecioCompra)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Venta>()
            .Property(v => v.Subtotal)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Venta>()
            .Property(v => v.Impuesto)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Venta>()
            .Property(v => v.Total)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.PrecioUnitario)
            .HasColumnType("decimal(18,2)");

        // Ignorar propiedad calculada (no es columna en BD)
        modelBuilder.Entity<DetalleVenta>()
            .Ignore(d => d.Subtotal);

        // Seed: datos iniciales
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "General", Descripcion = "Categoría por defecto", CreadoEn = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Categoria { Id = 2, Nombre = "Electrónica", Descripcion = "Productos electrónicos", CreadoEn = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Categoria { Id = 3, Nombre = "Ropa", Descripcion = "Prendas de vestir", CreadoEn = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }

    // Actualizar ModificadoEn automáticamente al guardar
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.ModificadoEn = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}