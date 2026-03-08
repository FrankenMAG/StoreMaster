using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;

namespace StoreMaster.Core.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<IEnumerable<Categoria>> GetActivasAsync()
            => await _repository.GetActivasAsync();

        public async Task<Categoria?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<(bool Success, string Message)> CreateAsync(Categoria categoria)
        {
            // Regla: no puede existir otra categoría con el mismo nombre
            if (await _repository.ExisteNombreAsync(categoria.Nombre))
                return (false, $"Ya existe una categoría con el nombre '{categoria.Nombre}'.");

            await _repository.AddAsync(categoria);
            return (true, $"Categoría '{categoria.Nombre}' creada correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Categoria categoria)
        {
            // Regla: no puede existir otra categoría con el mismo nombre
            if (await _repository.ExisteNombreAsync(categoria.Nombre, categoria.Id))
                return (false, $"Ya existe una categoría con el nombre '{categoria.Nombre}'.");

            categoria.ModificadoEn = DateTime.UtcNow;
            await _repository.UpdateAsync(categoria);
            return (true, $"Categoría '{categoria.Nombre}' actualizada correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null)
                return (false, "Categoría no encontrada.");

            // Regla: no eliminar la categoría General (Id = 1)
            if (categoria.Id == 1)
                return (false, "La categoría 'General' no puede eliminarse.");

            // Soft delete: solo marcar como eliminada
            categoria.Eliminado = true;
            await _repository.UpdateAsync(categoria);
            return (true, $"Categoría '{categoria.Nombre}' eliminada correctamente.");
        }
    }
}
