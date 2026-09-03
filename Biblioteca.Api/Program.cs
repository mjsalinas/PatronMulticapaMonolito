using Biblioteca.Application.Interfaces.Repositories;
using Biblioteca.Application.Services;
using Biblioteca.Domain.Validation;
using Biblioteca.Infrastructure.Persistence;
using Biblioteca.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistencia (EF Core + SQLite)
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Repositorios (Infrastructure -> Application.Interfaces)
builder.Services.AddScoped<IAutorRepository, AutorRepository>();
builder.Services.AddScoped<ILibroRepository, LibroRepository>();

// Validadores de dominio (puros, sin dependencias de infraestructura)
builder.Services.AddScoped<IAutorValidator, AutorValidator>();
builder.Services.AddScoped<ILibroValidator, LibroValidator>();

// Utilidades compartidas de Application
builder.Services.AddScoped<Biblioteca.Application.Common.ITextNormalizer, Biblioteca.Application.Common.TextNormalizer>();

// Servicios de aplicacion (orquestan validadores + repositorios)
builder.Services.AddScoped<AutorService>();
builder.Services.AddScoped<LibroService>();

var app = builder.Build();

// Aplica migraciones pendientes al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
