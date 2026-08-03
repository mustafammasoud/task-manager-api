using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Repositories;

/// <summary>
/// EF Core / PostgreSQL-backed implementation of ITaskRepository. Registered
/// as Scoped (not Singleton like InMemoryTaskRepository was) because it wraps
/// a Scoped DbContext — one instance per HTTP request. Kept synchronous to
/// match the existing interface so nothing above the repository layer
/// (service, controller) needs to change.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public TaskItem Add(TaskItem task)
    {
        _db.Tasks.Add(task);
        _db.SaveChanges();
        return task;
    }

    public IEnumerable<TaskItem> GetAll(TaskState? statusFilter = null)
    {
        var query = _db.Tasks.AsQueryable();
        if (statusFilter is not null)
        {
            query = query.Where(t => t.Status == statusFilter);
        }
        return query.OrderBy(t => t.CreatedAt).ToList();
    }

    public TaskItem? GetById(Guid id)
    {
        return _db.Tasks.Find(id);
    }

    public TaskItem? Update(Guid id, TaskItem updated)
    {
        var existing = _db.Tasks.Find(id);
        if (existing is null)
        {
            return null;
        }

        existing.Title = updated.Title;
        existing.Description = updated.Description;
        existing.Status = updated.Status;
        existing.CreatedAt = updated.CreatedAt;

        _db.SaveChanges();
        return existing;
    }

    public bool Delete(Guid id)
    {
        var existing = _db.Tasks.Find(id);
        if (existing is null)
        {
            return false;
        }

        _db.Tasks.Remove(existing);
        _db.SaveChanges();
        return true;
    }

    public bool Exists(Guid id)
    {
        return _db.Tasks.Any(t => t.Id == id);
    }
}
