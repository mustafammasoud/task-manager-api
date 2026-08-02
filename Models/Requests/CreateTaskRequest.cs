using System.ComponentModel.DataAnnotations;
using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Models.Requests;

/// <summary>Request body for POST /tasks. Title is mandatory.</summary>
public class CreateTaskRequest
{
    [Required(ErrorMessage = "title is required")]
    [MinLength(1, ErrorMessage = "title must not be empty")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskState Status { get; set; } = TaskState.Pending;
}
