using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http; // Для IFormFile

namespace WebApplication1.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IFileStorageService _fileStorageService; // <-- НОВОЕ ПОЛЕ

    public TeamController(ITeamService teamService, IFileStorageService fileStorageService)
    {
        _teamService = teamService;
        _fileStorageService = fileStorageService; 
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
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetTeamAssigmentsAssignments(int id)
    {
        //TODO тот самый TeamAssigment
        // var assignments = await _teamService.GetAssignmentsInTeamAsync(id);
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromForm] string name,
        [FromForm] int leaderId,
        [FromForm] string? description,
        IFormFile? avatarFile)
    {
        try
        {
            // Сохраняем файл (если есть) и получаем путь
            string? avatarUrl = await _fileStorageService.SaveFileAsync(avatarFile, "teams");

            var input = new TeamInput(
                LeaderId: leaderId,
                teamId: 0, // При создании ID ещё нет
                Name: name,
                Description: description,
                AvatarUrl: avatarUrl
            );

            await _teamService.CreateAsync(input);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Update(
        [FromForm] int teamId,
        [FromForm] string name,
        [FromForm] string? description,
        IFormFile? avatarFile)
    {
        try
        {
            var currentTeam = await _teamService.GetByIdAsync(teamId);
            if (currentTeam == null)
                return NotFound("Команда не найдена");

            string? finalAvatarUrl = currentTeam.AvatarUrl;

            // Если пришёл новый файл — заменяем старый
            if (avatarFile != null && avatarFile.Length > 0)
            {
                // Удаляем старый файл (если был)
                if (!string.IsNullOrEmpty(currentTeam.AvatarUrl))
                {
                    await _fileStorageService.DeleteFileAsync(currentTeam.AvatarUrl);
                }

                // Сохраняем новый
                finalAvatarUrl = await _fileStorageService.SaveFileAsync(avatarFile, "teams");
            }
            // Если файл не пришёл — оставляем старую аватарку

            var input = new TeamInput(
                LeaderId: currentTeam.LeaderId ?? 0,
                teamId: teamId,
                Name: name,
                Description: description,
                AvatarUrl: finalAvatarUrl
            );

            await _teamService.UpdateAsync(input);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _teamService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/users/{userId}")]
    public async Task<ActionResult> AddUser(int teamId, string email)
    {
        try
        {
            await _teamService.AddUserToTeamAsync(email, teamId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}/users/{userId}")]
    public async Task<ActionResult> RemoveUserFromTeam(int teamId, int userId)
    {
        try
        {
            await _teamService.RemoveUserFromTeamAsync(teamId, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/leader/{userId}")]
    public async Task<ActionResult> SetLeader(int teamId, int userId)
    {
        try
        {
            await _teamService.SetTeamLeaderAsync(teamId, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}