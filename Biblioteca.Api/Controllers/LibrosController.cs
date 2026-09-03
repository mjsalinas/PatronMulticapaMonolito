using Biblioteca.Application.Common;
using Biblioteca.Application.Services;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly LibroService _libroService;

    public LibrosController(LibroService libroService)
    {
        _libroService = libroService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var (_, _, libros) = await _libroService.ObtenerTodosAsync();
        return Ok(libros);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var (estado, mensaje, libro) = await _libroService.ObtenerPorIdAsync(id);

        return estado switch
        {
            EstadoResultado.Exito => Ok(libro),
            EstadoResultado.NoEncontrado => NotFound(mensaje),
            _ => BadRequest(mensaje)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Libro libro)
    {
        var (estado, mensaje, creado) = await _libroService.CrearAsync(libro);

        return estado switch
        {
            EstadoResultado.Exito => CreatedAtAction(nameof(GetById), new { id = creado!.Id }, creado),
            EstadoResultado.Invalido => BadRequest(mensaje),
            EstadoResultado.Conflicto => Conflict(mensaje),
            _ => BadRequest(mensaje)
        };
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Libro libro)
    {
        var (estado, mensaje, actualizado) = await _libroService.ActualizarAsync(id, libro);

        return estado switch
        {
            EstadoResultado.Exito => Ok(actualizado),
            EstadoResultado.NoEncontrado => NotFound(mensaje),
            EstadoResultado.Invalido => BadRequest(mensaje),
            EstadoResultado.Conflicto => Conflict(mensaje),
            _ => BadRequest(mensaje)
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (estado, mensaje) = await _libroService.EliminarAsync(id);

        return estado switch
        {
            EstadoResultado.Exito => NoContent(),
            EstadoResultado.NoEncontrado => NotFound(mensaje),
            EstadoResultado.Conflicto => Conflict(mensaje),
            _ => BadRequest(mensaje)
        };
    }
}
