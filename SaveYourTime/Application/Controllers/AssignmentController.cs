using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Services;

namespace WebApplication1.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(IAssignmentService assignmentService) =>
        _assignmentService = assignmentService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetAll() =>
        Ok(await _assignmentService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<AssignmentResponse>> GetById(int id)
    {
        var assignment = await _assignmentService.GetByIdAsync(id);
        if (assignment == null) return NotFound($"Задача с ID {id} не найдена");
        return Ok(assignment);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetByFilter(
        [FromQuery] string? title,
        [FromQuery] int? statusId,
        [FromQuery] int? userId)
    {
        var assignments = await _assignmentService.GetByFilterAsync(title, statusId, userId);
        return Ok(assignments);
    }

    [HttpPost]
    public async Task<ActionResult<AssignmentResponse>> Create([FromBody] AssignmentInput input)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Пользователь не авторизован");

            var userId = int.Parse(userIdClaim);
            var assignment = await _assignmentService.CreateAsync(input, userId);
            return CreatedAtAction(nameof(GetById), new { id = assignment.Id }, assignment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AssignmentResponse>> Update(int id, [FromBody] AssignmentInput input)
    {
        try
        {
            var assignment = await _assignmentService.UpdateAsync(id, input);
            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<AssignmentResponse>> UpdateStatus(int id, [FromBody] string status)
    {
        try
        {
            var assignment = await _assignmentService.UpdateStatusAsync(id, status);
            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/owner")]
    public async Task<ActionResult<AssignmentResponse>> ChangeOwner(int id, [FromBody] int newUserId)
    {
        try
        {
            var assignment = await _assignmentService.ChangeOwnerAsync(id, newUserId);
            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/content")]
    public async Task<ActionResult<AssignmentResponse>> UpdateContent(int id, [FromBody] UpdateContentInput input)
    {
        try
        {
            var assignment = await _assignmentService.UpdateContentAsync(id, input.Title, input.Description);
            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record UpdateContentInput(string Title, string? Description);