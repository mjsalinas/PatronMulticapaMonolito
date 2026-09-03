using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Interfaces;

public interface IAutorRepository
{
    Task<List<Autor>> GetAllAsync();
    Task<Autor?> GetByIdAsync(int id);
    Task AddAsync(Autor autor);
    Task UpdateAsync(Autor autor);
    Task DeleteAsync(Autor autor);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsDuplicadoAsync(string nombre, int? idExcluir = null);
}
