using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent_Project.Migrations
{
    /// <inheritdoc />
    public partial class alterposttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_LandlordId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_LandlordId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LandlordId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "User_id",
                table: "Posts",
                newName: "Landlord_id");

            migrationBuilder.AlterColumn<string>(
                name: "Landlord_name",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Landlord_id",
                table: "Posts",
                column: "Landlord_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_Landlord_id",
                table: "Posts",
                column: "Landlord_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_Landlord_id",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Landlord_id",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Landlord_id",
                table: "Posts",
                newName: "User_id");

            migrationBuilder.AlterColumn<int>(
                name: "Landlord_name",
                table: "Posts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "LandlordId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LandlordId",
                table: "Posts",
                column: "LandlordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_LandlordId",
                table: "Posts",
                column: "LandlordId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
