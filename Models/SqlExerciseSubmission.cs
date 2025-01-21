using System.ComponentModel.DataAnnotations;

namespace SqlLearningApp.Models
{
    public class SqlExerciseSubmission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? SubmittedQuery { get; set; }
        public bool IsCorrect { get; set; }
        [Required]
        public int SqlExerciseId { get; set; }
        public virtual SqlExercise? SqlExercise { get; set; }
    }
}
