using Biblioteca.Application.Interfaces.Repositories;
using Biblioteca.Domain.Entities;
using Biblioteca.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Repositories;

public class AutorRepository : IAutorRepository
{
    private readonly LibraryDbContext _context;

    public AutorRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Autor>> GetAllAsync()
    {
        var autores = await _context.Autores.ToListAsync();
        return autores
            .OrderBy(a => a.Nombre, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    public async Task<Autor?> GetByIdAsync(int id)
    {
        return await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Autor autor)
    {
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Autor autor)
    {
        _context.Autores.Update(autor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Autor autor)
    {
        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Autores.AnyAsync(a => a.Id == id);
    }

    public async Task<bool> ExistsDuplicadoAsync(string nombre, int? idExcluir = null)
    {
        return await _context.Autores.AnyAsync(a =>
            a.Nombre.ToLower() == nombre.ToLower() &&
            (idExcluir == null || a.Id != idExcluir));
    }
}
