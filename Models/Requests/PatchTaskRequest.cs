using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Models.Requests;

/// <summary>
/// Request body for PATCH /tasks/{id}. Every field is optional — only
/// properties that are supplied get applied. If Title is supplied it still
/// must not be blank; that rule is enforced in the service layer since
/// "optional but non-blank-if-present" isn't expressible with [Required].
/// </summary>
public class PatchTaskRequest
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public TaskState? Status { get; set; }
}
