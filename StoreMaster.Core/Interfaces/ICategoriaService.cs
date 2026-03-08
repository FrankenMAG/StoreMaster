using StoreMaster.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<IEnumerable<Categoria>> GetActivasAsync();
        Task<Categoria?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Categoria categoria);
        Task<(bool Success, string Message)> UpdateAsync(Categoria categoria);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
