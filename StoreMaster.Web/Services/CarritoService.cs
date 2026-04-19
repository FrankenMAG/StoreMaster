using System.Text.Json;
using StoreMaster.Core.Entities;

namespace StoreMaster.Web.Services;

public class CarritoService
{
    private const string CarritoKey = "Carrito";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CarritoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public Carrito ObtenerCarrito()
    {
        var json = Session.GetString(CarritoKey);
        if (string.IsNullOrEmpty(json))
            return new Carrito();

        return JsonSerializer.Deserialize<Carrito>(json) ?? new Carrito();
    }

    public void GuardarCarrito(Carrito carrito)
    {
        var json = JsonSerializer.Serialize(carrito);
        Session.SetString(CarritoKey, json);
    }

    public void AgregarItem(CarritoItem item)
    {
        var carrito = ObtenerCarrito();
        carrito.AgregarItem(item);
        GuardarCarrito(carrito);
    }

    public void EliminarItem(int productoId)
    {
        var carrito = ObtenerCarrito();
        carrito.EliminarItem(productoId);
        GuardarCarrito(carrito);
    }

    public void ActualizarCantidad(int productoId, int cantidad)
    {
        var carrito = ObtenerCarrito();
        carrito.ActualizarCantidad(productoId, cantidad);
        GuardarCarrito(carrito);
    }

    public void LimpiarCarrito()
    {
        var carrito = ObtenerCarrito();
        carrito.Limpiar();
        GuardarCarrito(carrito);
    }
}