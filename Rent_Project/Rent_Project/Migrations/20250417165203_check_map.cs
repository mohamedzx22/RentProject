using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent_Project.Migrations
{
    /// <inheritdoc />
    public partial class check_map : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Save_Post_Posts_PostId",
                table: "Save_Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Save_Post_Users_UserId",
                table: "Save_Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Save_Post",
                table: "Save_Post");

            migrationBuilder.RenameTable(
                name: "Save_Post",
                newName: "Save_Posts");

            migrationBuilder.RenameIndex(
                name: "IX_Save_Post_UserId",
                table: "Save_Posts",
                newName: "IX_Save_Posts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Save_Posts",
                table: "Save_Posts",
                columns: new[] { "PostId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Save_Posts_Posts_PostId",
                table: "Save_Posts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Save_Posts_Users_UserId",
                table: "Save_Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Save_Posts_Posts_PostId",
                table: "Save_Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Save_Posts_Users_UserId",
                table: "Save_Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Save_Posts",
                table: "Save_Posts");

            migrationBuilder.RenameTable(
                name: "Save_Posts",
                newName: "Save_Post");

            migrationBuilder.RenameIndex(
                name: "IX_Save_Posts_UserId",
                table: "Save_Post",
                newName: "IX_Save_Post_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Save_Post",
                table: "Save_Post",
                columns: new[] { "PostId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Save_Post_Posts_PostId",
                table: "Save_Post",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Save_Post_Users_UserId",
                table: "Save_Post",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
