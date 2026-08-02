namespace TaskManagerApi.Models;

/// <summary>
/// Task state. Named "TaskState" (not "TaskStatus") to avoid colliding with
/// System.Threading.Tasks.TaskStatus, which is implicitly in scope.
/// Serialized as lowercase "pending" / "completed" — see the JsonStringEnumConverter
/// with CamelCase naming policy registered in Program.cs.
/// </summary>
public enum TaskState
{
    Pending,
    Completed
}
