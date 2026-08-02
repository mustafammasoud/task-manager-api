namespace TaskManagerApi.Models.Entities;

/// <summary>
/// Task state. Named "TaskState" (not "TaskStatus") to avoid colliding with
/// System.Threading.Tasks.TaskStatus, which is implicitly in scope everywhere.
/// </summary>
public enum TaskState
{
    Pending,
    Completed
}
