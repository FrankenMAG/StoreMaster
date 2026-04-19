namespace StoreMaster.Core.Entities;

public class Cliente : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Apellido { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;

    public string? RFC { get; set; }
    public string? RazonSocial { get; set; }
    public string? RegimenFiscal { get; set; }
    public string? CodigoPostalFiscal { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    // Navegación
    public ICollection<Venta> Ventas { get; set; } = [];
}