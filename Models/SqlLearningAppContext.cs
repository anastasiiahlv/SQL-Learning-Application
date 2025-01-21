using Microsoft.EntityFrameworkCore;
using System.Data;

namespace SqlLearningApp.Models
{
    public class SqlLearningAppContext: DbContext
    {
        public SqlLearningAppContext(DbContextOptions<SqlLearningAppContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<SqlExercise> SqlExercises { get; set; }
        public virtual DbSet<SqlExerciseSubmission> SqlExerciseSubmissions { get; set; }
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SqlExercise>()
                .HasOne(e => e.Lesson)
                .WithMany(l => l.SqlExercises)
                .HasForeignKey(e => e.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
