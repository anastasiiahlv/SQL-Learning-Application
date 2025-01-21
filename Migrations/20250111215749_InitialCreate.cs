using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Learners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Learners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SqlExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectQuery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SqlExercises_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SqlExerciseSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmittedQuery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    SqlExerciseId = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlExerciseSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SqlExerciseSubmissions_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SqlExerciseSubmissions_SqlExercises_SqlExerciseId",
                        column: x => x.SqlExerciseId,
                        principalTable: "SqlExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Learners_Email",
                table: "Learners",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Learners_Username",
                table: "Learners",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SqlExercises_LessonId",
                table: "SqlExercises",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_SqlExerciseSubmissions_LearnerId",
                table: "SqlExerciseSubmissions",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SqlExerciseSubmissions_SqlExerciseId",
                table: "SqlExerciseSubmissions",
                column: "SqlExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "SqlExerciseSubmissions");

            migrationBuilder.DropTable(
                name: "Learners");

            migrationBuilder.DropTable(
                name: "SqlExercises");

            migrationBuilder.DropTable(
                name: "Lessons");
        }
    }
}
