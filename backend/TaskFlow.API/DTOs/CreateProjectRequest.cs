using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs;

public class CreateProjectRequest
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
}