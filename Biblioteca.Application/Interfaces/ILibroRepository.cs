using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Interfaces;

public interface ILibroRepository
{
    Task<List<Libro>> GetAllAsync();
    Task<Libro?> GetByIdAsync(int id);
    Task AddAsync(Libro libro);
    Task UpdateAsync(Libro libro);
    Task DeleteAsync(Libro libro);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByAutorIdAsync(int autorId);
}
