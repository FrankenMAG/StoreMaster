using System;
using System.Collections.Generic;
using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces
{
    public interface ICategoriaRepository : IGenericRepository<Categoria>
    {
        // Metodos especificos de cada categoria
        Task<IEnumerable<Categoria>> GetActivasAsync();
        Task<bool> ExisteNombreAsync(string  nombre,int? excludeId=null);
    }
}
