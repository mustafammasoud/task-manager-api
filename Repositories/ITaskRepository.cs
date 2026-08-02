using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Repositories;

/// <summary>
/// Persistence contract for tasks. Deliberately dumb: no validation, no
/// business rules — just storage operations. Swap InMemoryTaskRepository for
/// an EF Core-backed implementation without touching the service layer.
/// </summary>
public interface ITaskRepository
{
    TaskItem Add(TaskItem task);
    IEnumerable<TaskItem> GetAll(TaskState? statusFilter = null);
    TaskItem? GetById(Guid id);
    TaskItem? Update(Guid id, TaskItem updated);
    bool Delete(Guid id);
    bool Exists(Guid id);
}
