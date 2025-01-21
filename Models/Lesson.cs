using System.ComponentModel.DataAnnotations;

namespace SqlLearningApp.Models
{
    public class Lesson
    {
        public Lesson()
        {
            SqlExercises = new List<SqlExercise>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public virtual ICollection<SqlExercise> SqlExercises { get; set; }
    }
}
