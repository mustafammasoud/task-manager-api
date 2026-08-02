using System.Collections.Concurrent;
using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Repositories;

/// <summary>
/// In-memory store, registered as a singleton so data survives across
/// requests for the lifetime of the process. Only concern: CRUD against the
/// dictionary. No validation or HTTP-shaped logic belongs here.
/// </summary>
public class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();

    public TaskItem Add(TaskItem task)
    {
        _tasks[task.Id] = task;
        return task;
    }

    public IEnumerable<TaskItem> GetAll(TaskState? statusFilter = null)
    {
        var items = _tasks.Values.AsEnumerable();
        if (statusFilter is not null)
        {
            items = items.Where(t => t.Status == statusFilter);
        }
        return items.OrderBy(t => t.CreatedAt).ToList();
    }

    public TaskItem? GetById(Guid id)
    {
        return _tasks.TryGetValue(id, out var task) ? task : null;
    }

    public TaskItem? Update(Guid id, TaskItem updated)
    {
        if (!_tasks.ContainsKey(id))
        {
            return null;
        }
        _tasks[id] = updated;
        return updated;
    }

    public bool Delete(Guid id)
    {
        return _tasks.TryRemove(id, out _);
    }

    public bool Exists(Guid id)
    {
        return _tasks.ContainsKey(id);
    }
}
