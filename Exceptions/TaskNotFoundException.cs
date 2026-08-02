namespace TaskManagerApi.Exceptions;

/// <summary>Thrown when a requested task id doesn't exist. Mapped to 404 by ExceptionHandlingMiddleware.</summary>
public class TaskNotFoundException : Exception
{
    public Guid TaskId { get; }

    public TaskNotFoundException(Guid taskId)
        : base($"Task with id '{taskId}' not found")
    {
        TaskId = taskId;
    }
}
