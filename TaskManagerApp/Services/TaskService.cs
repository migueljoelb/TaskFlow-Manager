using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener tareas de un usuario
        public async Task<List<TaskItem>> GetTasksByUserAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        // Crear tarea
        public async Task CreateTaskAsync(TaskItem task)
        {
            // Validar que el usuario exista antes de intentar guardar
            var userExists = await _context.Users.AnyAsync(u => u.Id == task.UserId);
            if (!userExists)
                throw new ArgumentException($"UserId {task.UserId} no existe.", nameof(task.UserId));

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        // Obtener tarea por Id
        public async Task<TaskItem?> GetByIdAsync(int id, int userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        // Editar tarea
        public async Task<bool> UpdateTaskAsync(TaskItem task)
        {
            var existing = await _context.Tasks.FindAsync(task.Id);
            if (existing == null) return false;

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.Status = task.Status;
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar tarea
        public async Task<bool> DeleteTaskAsync(int id, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
