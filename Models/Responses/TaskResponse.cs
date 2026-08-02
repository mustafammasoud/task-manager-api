using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Models.Responses;

/// <summary>The shape of a task as returned by the API.</summary>
public class TaskResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskState Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
