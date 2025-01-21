using System.ComponentModel.DataAnnotations;

namespace SqlLearningApp.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Ціна не може бути від'ємною.")]
        public double Price { get; set; }
        [Required]
        public string? Author { get; set; }
    }
}
