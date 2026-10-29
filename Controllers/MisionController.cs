using Microsoft.AspNetCore.Mvc;
using DSW_3P_PR01.Services;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class MisionController : ControllerBase
{
    private readonly IMisionService _service;
    
    public MisionController(IMisionService service)
    {
        _service = service;
    }

    //GET - Obtener todas las misiones
    [HttpGet]
    [ProducesResponseType(typeof(List<Mision>),200)]
    public IActionResult ObtenerTodas()
    {
        return Ok(_service.ObtenerTodas());
        
    }

    //GET - Obtener mision por ID
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(List<Mision>), 200)]
    [ProducesResponseType(404)]
    public IActionResult ObtenerPorId(int id)
    {
        var Mision = _service.ObtenerPorId(id);

        if(Mision == null)
        {
            return NotFound();
        }
        return Ok(Mision);
    }

    //POST - Agregar Mision
    [HttpPost]
    [ProducesResponseType(typeof(Mision), 200)]
    [ProducesResponseType(400)]
    public IActionResult Agregar([FromBody] Mision mision)
    {
        try
        {
            var creada = _service.Agregar(mision);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new {id = creada.Id},
                creada
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new {mensaje = ex.Message});
        }
    }

    //PUT - Actualizar Mision
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult Actualizar(int id, [FromBody] Mision mision)
    {
        try
        {
            if (!_service.Actualizar(id, mision))
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new {mensaje = ex.Message});
        }
    }

    //DELETE - Eliminar Mision
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Eliminar(int id)
    {
        if (!_service.Eliminar(id))
        {
            return NotFound();
        }
        return NoContent();
    }
}