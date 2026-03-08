namespace StoreMaster.Core.Entities;

public class Producto : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? CodigoBarras { get; set; }
    public decimal Precio { get; set; }
    public decimal? PrecioCompra { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; } = 5;
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; } = true;

    // FK
    public int CategoriaId { get; set; }
    public int ProveedorId { get; set; }

    // Navegación
    public Categoria Categoria { get; set; } = null!;
    public Proveedor Proveedor { get; set; } = null!;
    public ICollection<DetalleVenta> DetallesVenta { get; set; } = [];
}