using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class DeleteLearnerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SqlExerciseSubmissions_Learners_LearnerId",
                table: "SqlExerciseSubmissions");

            migrationBuilder.DropTable(
                name: "Learners");

            migrationBuilder.DropIndex(
                name: "IX_SqlExerciseSubmissions_LearnerId",
                table: "SqlExerciseSubmissions");

            migrationBuilder.DropColumn(
                name: "LearnerId",
                table: "SqlExerciseSubmissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LearnerId",
                table: "SqlExerciseSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Learners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Learners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SqlExerciseSubmissions_LearnerId",
                table: "SqlExerciseSubmissions",
                column: "LearnerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SqlExerciseSubmissions_Learners_LearnerId",
                table: "SqlExerciseSubmissions",
                column: "LearnerId",
                principalTable: "Learners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
