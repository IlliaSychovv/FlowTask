using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexOnTaskId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Id",
                table: "Tasks",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_Id",
                table: "Tasks");
        }
    }
}
