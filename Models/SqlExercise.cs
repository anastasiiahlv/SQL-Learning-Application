using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace SqlLearningApp.Models
{
    public class SqlExercise
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? CorrectQuery { get; set; }
        [Required]
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; } = null!;

    }
}
