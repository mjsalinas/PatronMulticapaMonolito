using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Persistence;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Libro> Libros => Set<Libro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Nombre).IsRequired();
            entity.Property(a => a.Nacionalidad).IsRequired();
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Titulo).IsRequired();
            entity.HasOne(l => l.Autor)
                  .WithMany()
                  .HasForeignKey(l => l.AutorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }
}
