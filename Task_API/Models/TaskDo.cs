using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_API.Models
{
    public class TaskDo
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public int StatusId { get; set; }

    }
}
