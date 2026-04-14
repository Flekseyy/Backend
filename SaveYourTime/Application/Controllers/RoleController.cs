using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Services;

namespace WebApplication1.Application.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleResponse>> GetById(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
            return NotFound($"Роль с ID {id} не найдена");
        
        return Ok(role);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> GetByFilter([FromQuery] string? name)
    {
        var roles = await _roleService.GetByFilterAsync(name);
        return Ok(roles);
    }

    [HttpPost]
    public async Task<ActionResult<RoleResponse>> Create([FromBody] RoleInput input)
    {
        try
        {
            var role = await _roleService.CreateAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleResponse>> Update(int id, [FromBody] RoleInput input)
    {
        try
        {
            var role = await _roleService.UpdateAsync(id, input);
            return Ok(role);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roleService.DeleteAsync(id);
        return NoContent();
    }
}