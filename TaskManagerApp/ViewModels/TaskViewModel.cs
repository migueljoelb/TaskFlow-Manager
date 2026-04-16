using System.ComponentModel.DataAnnotations;
using TaskManagerApp.Models;

namespace TaskManagerApp.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public Models.TaskStatus Status { get; set; }
    }
}
