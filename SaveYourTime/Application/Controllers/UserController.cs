using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Services;

namespace WebApplication1.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound($"Пользователь с ID {id} не найден");
        
        return Ok(user);
    }
    
    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetByTeam(int teamId)
    {
        var users = await _userService.GetByTeamIdAsync(teamId);
        return Ok(users);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetByFilter(
        [FromQuery] string? username,
        [FromQuery] int? roleId)
    {
        var users = await _userService.GetByFilterAsync(username, roleId);
        return Ok(users);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create([FromBody] UserInput input)
    {
        try
        {
            var user = await _userService.CreateAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UserInput input)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, input);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpPatch("{id}/role")]
    public async Task<ActionResult<UserResponse>> ChangeRole(int id, [FromBody] int? roleId)
    {
        try
        {
            var user = await _userService.ChangeRoleAsync(id, roleId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/team")]
    public async Task<ActionResult<UserResponse>> AddToTeam(int id, [FromBody] int teamId)
    {
        try
        {
            var user = await _userService.AddToTeamAsync(id, teamId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPatch("{id}/team/remove")]
    public async Task<ActionResult<UserResponse>> RemoveFromTeam(int id)
    {
        try
        {
            var user = await _userService.RemoveFromTeamAsync(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}