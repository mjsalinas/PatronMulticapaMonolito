using Biblioteca.Application.Common;
using Biblioteca.Application.Services;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly AutorService _autorService;

    public AutoresController(AutorService autorService)
    {
        _autorService = autorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var (_, _, autores) = await _autorService.ObtenerTodosAsync();
        return Ok(autores);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var (estado, mensaje, autor) = await _autorService.ObtenerPorIdAsync(id);

        return estado switch
        {
            EstadoResultado.Exito => Ok(autor),
            EstadoResultado.NoEncontrado => NotFound(mensaje),
            _ => BadRequest(mensaje)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Autor autor)
    {
        var (estado, mensaje, creado) = await _autorService.CrearAsync(autor);

        return estado switch
        {
            EstadoResultado.Exito => CreatedAtAction(nameof(GetById), new { id = creado!.Id }, creado),
            EstadoResultado.Invalido => BadRequest(mensaje),
            EstadoResultado.Conflicto => Conflict(mensaje),
            _ => BadRequest(mensaje)
        };
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Autor autor)
    {
        var (estado, mensaje, actualizado) = await _autorService.ActualizarAsync(id, autor);

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
        var (estado, mensaje) = await _autorService.EliminarAsync(id);

        return estado switch
        {
            EstadoResultado.Exito => NoContent(),
            EstadoResultado.NoEncontrado => NotFound(mensaje),
            EstadoResultado.Conflicto => Conflict(mensaje),
            _ => BadRequest(mensaje)
        };
    }
}
