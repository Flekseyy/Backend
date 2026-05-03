using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Application.DTOs.Inputs.Assigments;

public record AssignmentInput(
        [Required]
        [StringLength(500, MinimumLength = 1)]
        string Title,

        // изменение длины, было 1000
        [StringLength(1500)] string? Description,

        [Required] [StringLength(20)] string Priority,
 

    DateTime? Deadline
);
