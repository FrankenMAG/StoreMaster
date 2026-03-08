using Microsoft.AspNetCore.Identity;

namespace StoreMaster.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    // Propiedad calculada: nombre completo
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
}