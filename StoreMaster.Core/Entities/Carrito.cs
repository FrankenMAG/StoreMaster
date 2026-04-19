using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Entities;
public class Carrito
{
    public List<CarritoItem> Items { get; set; } = [];

    public decimal Subtotal => Items.Sum(i => i.Subtotal);
    public decimal Impuesto => Subtotal * 0.16m; // 16% IVA
    public decimal Total => Subtotal + Impuesto;
    public int TotalItems => Items.Sum(i => i.Cantidad);

    public void AgregarItem(CarritoItem item)
    {
        var existente = Items.FirstOrDefault(i => i.ProductoId == item.ProductoId);
        if (existente != null)
        {
            // Si ya existe, solo aumenta la cantidad
            existente.Cantidad += item.Cantidad;
        }
        else
        {
            Items.Add(item);
        }
    }

    public void EliminarItem(int productoId)
        => Items.RemoveAll(i => i.ProductoId == productoId);

    public void ActualizarCantidad(int productoId, int cantidad)
    {
        var item = Items.FirstOrDefault(i => i.ProductoId == productoId);
        if (item != null)
        {
            if (cantidad <= 0)
                EliminarItem(productoId);
            else
                item.Cantidad = cantidad;
        }
    }

    public void Limpiar() => Items.Clear();
}