using StoreMaster.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Interfaces
{
    public interface IProveedorService
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<IEnumerable<Proveedor>> GetActivosAsync();
        Task<Proveedor?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Proveedor proveedor);
        Task<(bool Success, string Message)> UpdateAsync(Proveedor proveedor);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
