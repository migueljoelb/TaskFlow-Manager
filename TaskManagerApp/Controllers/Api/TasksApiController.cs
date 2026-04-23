using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Models;
using TaskManagerApp.Services;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Controllers.Api
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksApiController(TaskService taskService)
        {
            _taskService = taskService;
        }

        // ─────────────────────────────────────────
        // GET /api/tasks?userId=1
        // Retorna todas las tareas de un usuario
        // ─────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "UserId inválido" });

            var tasks = await _taskService.GetTasksByUserAsync(userId);

            var result = tasks.Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                Status = t.Status.ToString(),
                t.CreatedAt,
                t.UpdatedAt
            });

            return Ok(result);
        }

        // ─────────────────────────────────────────
        // GET /api/tasks/5?userId=1
        // Retorna una tarea por Id
        // ─────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int userId)
        {
            var task = await _taskService.GetByIdAsync(id, userId);

            if (task == null)
                return NotFound(new { message = "Tarea no encontrada" });

            return Ok(new
            {
                task.Id,
                task.Title,
                task.Description,
                Status = task.Status.ToString(),
                task.CreatedAt,
                task.UpdatedAt
            });
        }

        // ─────────────────────────────────────────
        // POST /api/tasks
        // Crea una nueva tarea
        // ─────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                Status = Models.TaskStatus.Pendiente,
                UserId = request.UserId
            };

            try
            {
                await _taskService.CreateTaskAsync(task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return CreatedAtAction(nameof(GetById),
                new { id = task.Id, userId = task.UserId },
                new { task.Id, task.Title, task.Description });
        }

        // ─────────────────────────────────────────
        // PUT /api/tasks/5
        // Actualiza una tarea existente
        // ─────────────────────────────────────────
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = new TaskItem
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                UserId = request.UserId
            };

            var success = await _taskService.UpdateTaskAsync(task);

            if (!success)
                return NotFound(new { message = "Tarea no encontrada" });

            return Ok(new { message = "Tarea actualizada correctamente" });
        }

        // ─────────────────────────────────────────
        // DELETE /api/tasks/5?userId=1
        // Elimina una tarea
        // ─────────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var success = await _taskService.DeleteTaskAsync(id, userId);

            if (!success)
                return NotFound(new { message = "Tarea no encontrada" });

            return Ok(new { message = "Tarea eliminada correctamente" });
        }
    }

    // ─────────────────────────────────────────
    // Clases para recibir datos del body JSON
    // ─────────────────────────────────────────
    public class CreateTaskRequest
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateTaskRequest
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public Models.TaskStatus Status { get; set; }
        public int UserId { get; set; }
    }
}