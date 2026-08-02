using System.ComponentModel.DataAnnotations;
using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Models.Requests;

/// <summary>
/// Request body for PUT /tasks/{id}. Represents a full replacement of the
/// resource, so — like create — Title is mandatory.
/// </summary>
public class ReplaceTaskRequest
{
    [Required(ErrorMessage = "title is required")]
    [MinLength(1, ErrorMessage = "title must not be empty")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskState Status { get; set; } = TaskState.Pending;
}
