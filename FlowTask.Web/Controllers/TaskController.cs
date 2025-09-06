using FlowTask.Application.DTO;
using FlowTask.Application.DTO.Task;
using FlowTask.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowTask.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] CreateTaskDto dto)
    {
        var task = await _taskService.CreateTaskAsync(dto, User);
        return CreatedAtAction(nameof(Create), new { id = task.Id }, task);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskByIdAsync(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id, User);
        return Ok(task);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<TaskDto>>> GetTasksAsync([FromQuery] TaskFilterDto? filter = null, int pageNumber = 1, int pageSize = 10)
    {
        var tasks = await _taskService.GetTasksAsync(User, filter, pageNumber, pageSize);
        return Ok(tasks);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaskAsync(Guid id, [FromQuery] UpdateTaskDto dto)
    {
        var updated = await _taskService.UpdateTaskAsync(id, dto, User);
        if (!updated)
            return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskAsync(Guid id)
    {
        var task = await _taskService.DeleteTaskAsync(id, User);
        if (!task)
            return NotFound();
        
        return NoContent();
    }
}