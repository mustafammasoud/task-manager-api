using TaskManagerApi.Models.Entities;
using TaskManagerApi.Models.Requests;
using TaskManagerApi.Models.Responses;

namespace TaskManagerApi.Mapping;

/// <summary>
/// Pure mapping functions between the domain entity and the API-facing
/// request/response DTOs. No business logic lives here — just shape translation.
/// </summary>
public static class TaskMapper
{
    public static TaskResponse ToResponse(TaskItem entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        Status = entity.Status,
        CreatedAt = entity.CreatedAt
    };

    public static TaskItem ToEntity(CreateTaskRequest request) => new()
    {
        Id = Guid.NewGuid(),
        Title = request.Title.Trim(),
        Description = request.Description,
        Status = request.Status,
        CreatedAt = DateTime.UtcNow
    };

    /// <summary>Builds a replacement entity for PUT, preserving Id/CreatedAt from the existing record.</summary>
    public static TaskItem ToEntity(ReplaceTaskRequest request, TaskItem existing) => new()
    {
        Id = existing.Id,
        CreatedAt = existing.CreatedAt,
        Title = request.Title.Trim(),
        Description = request.Description,
        Status = request.Status
    };

    /// <summary>Applies only the fields present on a PATCH request onto a copy of the existing entity.</summary>
    public static TaskItem ApplyPatch(PatchTaskRequest request, TaskItem existing) => new()
    {
        Id = existing.Id,
        CreatedAt = existing.CreatedAt,
        Title = string.IsNullOrWhiteSpace(request.Title) ? existing.Title : request.Title.Trim(),
        Description = request.Description ?? existing.Description,
        Status = request.Status ?? existing.Status
    };
}
