using TaskManagerApi.Models.Entities;
using TaskManagerApi.Models.Requests;
using TaskManagerApi.Models.Responses;

namespace TaskManagerApi.Services;

/// <summary>
/// Business operations for tasks. Controllers depend on this, not on the
/// repository directly — keeps HTTP concerns (status codes) out of business
/// logic, and business logic out of the controller.
/// </summary>
public interface ITaskService
{
    TaskResponse CreateTask(CreateTaskRequest request);
    IEnumerable<TaskResponse> GetAllTasks(TaskState? statusFilter = null);
    TaskResponse GetTaskById(Guid id);
    TaskResponse ReplaceTask(Guid id, ReplaceTaskRequest request);
    TaskResponse UpdateTask(Guid id, PatchTaskRequest request);
    void DeleteTask(Guid id);
}
