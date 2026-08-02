namespace TaskManagerApi.Models.Entities;

/// <summary>
/// The internal domain entity for a task. Kept separate from the API-facing
/// Request/Response models so the wire format can change independently of
/// the storage/domain shape.
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public TaskState Status { get; set; } = TaskState.Pending;

    public DateTime CreatedAt { get; set; }
}
