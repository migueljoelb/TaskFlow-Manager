using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Models;
using TaskManagerApp.Services;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        // Obtener el usuario de la sesión
        private int? GetUserId() => HttpContext.Session.GetInt32("UserId");

        // GET: /Tasks
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var tasks = await _taskService.GetTasksByUserAsync(userId.Value);
            return View(tasks);
        }

        // GET: /Tasks/Create
        public IActionResult Create()
        {
            if (GetUserId() == null) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: /Tasks/Create
        [HttpPost]
        public async Task<IActionResult> Create(TaskViewModel model)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(model);

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                UserId = userId.Value
            };

            await _taskService.CreateTaskAsync(task);
            return RedirectToAction("Index");
        }

        // GET: /Tasks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var task = await _taskService.GetByIdAsync(id, userId.Value);
            if (task == null) return NotFound();

            var model = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status
            };

            return View(model);
        }

        // POST: /Tasks/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(TaskViewModel model)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(model);

            var task = new TaskItem
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                UserId = userId.Value
            };

            await _taskService.UpdateTaskAsync(task);
            return RedirectToAction("Index");
        }

        // POST: /Tasks/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            await _taskService.DeleteTaskAsync(id, userId.Value);
            return RedirectToAction("Index");
        }
    }
}
