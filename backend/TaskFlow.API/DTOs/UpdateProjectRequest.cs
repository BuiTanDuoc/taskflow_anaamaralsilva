using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs;

public class UpdateProjectRequest
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public string Status { get; set; } = "Active";
}