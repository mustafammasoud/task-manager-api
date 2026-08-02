using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models;

/// <summary>Payload for POST /tasks — title is mandatory.</summary>
public class TaskCreateDto
{
    [Required(ErrorMessage = "title is required")]
    [MinLength(1, ErrorMessage = "title must not be empty")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskState Status { get; set; } = TaskState.Pending;
}

/// <summary>
/// Payload for PUT and PATCH /tasks/{id}.
/// All fields are nullable here so the same DTO can be reused for a partial (PATCH)
/// update; the PUT handler additionally enforces that Title is supplied, since PUT
/// represents a full replacement of the resource.
/// </summary>
public class TaskUpdateDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public TaskState? Status { get; set; }
}
