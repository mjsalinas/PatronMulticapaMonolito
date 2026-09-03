using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly LibraryDbContext _context;

    public LibroRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Libro>> GetAllAsync()
    {
        var libros = await _context.Libros
            .Include(l => l.Autor)
            .ToListAsync();

        return libros
            .OrderBy(l => l.Titulo, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    public async Task<Libro?> GetByIdAsync(int id)
    {
        return await _context.Libros
            .Include(l => l.Autor)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddAsync(Libro libro)
    {
        _context.Libros.Add(libro);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Libro libro)
    {
        _context.Libros.Update(libro);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Libro libro)
    {
        _context.Libros.Remove(libro);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Libros.AnyAsync(l => l.Id == id);
    }

    public async Task<bool> ExistsByAutorIdAsync(int autorId)
    {
        return await _context.Libros.AnyAsync(l => l.AutorId == autorId);
    }
}
