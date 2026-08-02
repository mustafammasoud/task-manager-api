using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Models.Entities;
using TaskManagerApi.Models.Requests;
using TaskManagerApi.Models.Responses;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers;

/// <summary>
/// HTTP surface for task CRUD. Deliberately thin: validates nothing itself
/// beyond what [ApiController]'s automatic model validation already gives
/// it, makes no direct repository calls, and contains no business rules —
/// it just translates HTTP requests into service calls and service results
/// into HTTP responses. Domain errors are thrown as exceptions by the
/// service and turned into status codes by ExceptionHandlingMiddleware.
/// </summary>
[ApiController]
[Route("tasks")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>Create a new task.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TaskResponse> Create([FromBody] CreateTaskRequest request)
    {
        var created = _taskService.CreateTask(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Retrieve all tasks, optionally filtered by status.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponse>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<TaskResponse>> GetAll([FromQuery] TaskState? status)
    {
        return Ok(_taskService.GetAllTasks(status));
    }

    /// <summary>Retrieve a single task by id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TaskResponse> GetById(Guid id)
    {
        return Ok(_taskService.GetTaskById(id));
    }

    /// <summary>Full update (replace) of a task. Title is required.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TaskResponse> Replace(Guid id, [FromBody] ReplaceTaskRequest request)
    {
        return Ok(_taskService.ReplaceTask(id, request));
    }

    /// <summary>Partial update of a task. Only supplied fields are changed.</summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TaskResponse> Update(Guid id, [FromBody] PatchTaskRequest request)
    {
        return Ok(_taskService.UpdateTask(id, request));
    }

    /// <summary>Delete a task.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        _taskService.DeleteTask(id);
        return NoContent();
    }
}
