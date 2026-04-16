using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relación: un usuario tiene muchas tareas
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
