using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdIndexToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_Id",
                table: "Tasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Id",
                table: "Tasks",
                column: "Id");
        }
    }
}
