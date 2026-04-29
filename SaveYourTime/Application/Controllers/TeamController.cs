using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Services;

namespace WebApplication1.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamResponse>>> GetAll()
    {
        var teams = await _teamService.GetAllAsync();
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamResponse>> GetById(int id)
    {
        var team = await _teamService.GetByIdAsync(id);
        if (team == null)
            return NotFound($"Команда с ID {id} не найдена");
        
        return Ok(team);
    }

    [HttpGet("{id}/users")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetTeamUsers(int teamId)
    {
        var users = await _teamService.GetUsersInTeamAsync(teamId);
        return Ok(users);
    }

    [HttpGet("{id}/assignments")]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetAssignments(int id)
    {
        var assignments = await _teamService.GetAssignmentsInTeamAsync(id);
        return Ok(assignments);
    }
    
    [HttpPost]
    public async Task<ActionResult<TeamResponse>> Create([FromBody] TeamInput input)
    {
        try
        {
            var team = await _teamService.CreateAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = team.Id }, team);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TeamResponse>> Update(int id, [FromBody] TeamInput input)
    {
        try
        {
            var team = await _teamService.UpdateAsync(id, input);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _teamService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/users/{userId}")]
    public async Task<ActionResult<TeamResponse>> AddUser(int id, int userId)
    {
        try
        {
            var team = await _teamService.AddUserToTeamAsync(userId, id);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}/users/{userId}")]
    public async Task<ActionResult<TeamResponse>> RemoveUser(int id, int userId)
    {
        try
        {
            var team = await _teamService.RemoveUserFromTeamAsync(userId);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/leader/{userId}")]
    public async Task<ActionResult<TeamResponse>> SetLeader(int id, int userId)
    {
        try
        {
            var team = await _teamService.SetTeamLeaderAsync(id, userId);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/leader")]
    public async Task<ActionResult<TeamResponse>> ChangeLeader(int id, [FromBody] int newLeaderId)
    {
        try
        {
            var team = await _teamService.ChangeTeamLeaderAsync(id, newLeaderId);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}