namespace StoreMaster.Core.Entities;

public class Venta : BaseEntity
{
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }
    public EstadoVenta Estado { get; set; } = EstadoVenta.Completada;
    public string? Notas { get; set; }

    // FK
    public int? ClienteId { get; set; }

    // Navegación
    public Cliente? Cliente { get; set; }
    public ICollection<DetalleVenta> Detalles { get; set; } = [];
}

public enum EstadoVenta
{
    Pendiente,
    Completada,
    Cancelada,
    Devuelta
}