using TaskManagerApi.Exceptions;
using TaskManagerApi.Mapping;
using TaskManagerApi.Models.Entities;
using TaskManagerApi.Models.Requests;
using TaskManagerApi.Models.Responses;
using TaskManagerApi.Repositories;

namespace TaskManagerApi.Services;

/// <summary>
/// Implements task business rules: existence checks, the "title can't be
/// blank" rule for PATCH, and orchestrating the mapper + repository. Throws
/// domain exceptions (TaskNotFoundException / InvalidTaskDataException)
/// rather than returning HTTP status codes — that translation happens in
/// ExceptionHandlingMiddleware, keeping this class free of HTTP concerns.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public TaskResponse CreateTask(CreateTaskRequest request)
    {
        var entity = TaskMapper.ToEntity(request);
        _repository.Add(entity);
        return TaskMapper.ToResponse(entity);
    }

    public IEnumerable<TaskResponse> GetAllTasks(TaskState? statusFilter = null)
    {
        return _repository.GetAll(statusFilter).Select(TaskMapper.ToResponse);
    }

    public TaskResponse GetTaskById(Guid id)
    {
        var entity = _repository.GetById(id) ?? throw new TaskNotFoundException(id);
        return TaskMapper.ToResponse(entity);
    }

    public TaskResponse ReplaceTask(Guid id, ReplaceTaskRequest request)
    {
        var existing = _repository.GetById(id) ?? throw new TaskNotFoundException(id);

        var updatedEntity = TaskMapper.ToEntity(request, existing);
        _repository.Update(id, updatedEntity);
        return TaskMapper.ToResponse(updatedEntity);
    }

    public TaskResponse UpdateTask(Guid id, PatchTaskRequest request)
    {
        var existing = _repository.GetById(id) ?? throw new TaskNotFoundException(id);

        if (request.Title is not null && string.IsNullOrWhiteSpace(request.Title))
        {
            throw new InvalidTaskDataException("title cannot be empty");
        }

        var patchedEntity = TaskMapper.ApplyPatch(request, existing);
        _repository.Update(id, patchedEntity);
        return TaskMapper.ToResponse(patchedEntity);
    }

    public void DeleteTask(Guid id)
    {
        if (!_repository.Exists(id))
        {
            throw new TaskNotFoundException(id);
        }
        _repository.Delete(id);
    }
}
