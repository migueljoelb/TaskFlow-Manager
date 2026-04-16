using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.Models
{
    public enum TaskStatus
    {
        Pendiente,
        EnProceso,
        Completada
    }

    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Pendiente;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Relación con usuario
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
