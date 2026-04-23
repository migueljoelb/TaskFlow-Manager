using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksApiController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound(new { message = "Tarea no encontrada" });
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TaskItem model)
        {
            await _taskService.CreateTaskAsync(model);
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TaskItem model)
        {
            if (id != model.Id) return BadRequest();
            var updated = await _taskService.UpdateTaskAsync(model);
            if (!updated) return NotFound(new { message = "Tarea no encontrada" });
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted) return NotFound(new { message = "Tarea no encontrada" });
            return NoContent();
        }
    }
}
