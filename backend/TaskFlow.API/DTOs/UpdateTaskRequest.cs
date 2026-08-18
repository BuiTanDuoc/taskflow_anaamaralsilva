using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs;

public class UpdateTaskRequest
{
    [Required]
    [MinLength(2)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Pending";

    [Required]
    public string Priority { get; set; } = "Medium";

    [Required]
    public DateTime DueDate { get; set; }
}