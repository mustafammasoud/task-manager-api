namespace TaskManagerApi.Models;

/// <summary>
/// The core task record returned by the API.
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public TaskState Status { get; set; } = TaskState.Pending;

    public DateTime CreatedAt { get; set; }
}
