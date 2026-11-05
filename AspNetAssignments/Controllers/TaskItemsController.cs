using Microsoft.AspNetCore.Mvc;
using AspNetAssignments.DTOs.Project;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Controllers;

/// <summary>
/// Контролер для операцій із окремими задачами всередині проєктів
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemsController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    /// <summary>
    /// Отримує перелік усіх активних задач
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskItemService.GetAllAsync();
        return Ok(tasks);
    }

    /// <summary>
    /// Знаходить задачу за її унікальним ідентифікатором
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _taskItemService.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound(new { message = $"Задачу з ID {id} не знайдено." });
        }
        return Ok(task);
    }

    /// <summary>
    /// Створює нову задачу та прив'язує її до вказаного проєкту
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _taskItemService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Оновлює статус, пріоритет або текстовий опис задачі
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskItemDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _taskItemService.UpdateAsync(id, dto);
        if (!result)
        {
            return NotFound(new { message = $"Задачу з ID {id} не знайдено." });
        }

        return Ok(new { message = "Задачу успішно оновлено." });
    }

    /// <summary>
    /// Виконує м'яке видалення задачі
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _taskItemService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = $"Задачу з ID {id} не знайдено." });
        }

        return NoContent();
    }
}
