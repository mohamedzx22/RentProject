using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddPostSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          string query = @"INSERT INTO Posts (Description, Number_of_viewers, rental_status, Title, Accsepted_Status, images, location, Price, User_id, Landlord_name, LandlordId)
                 VALUES ('Test', 10, 0, 'ehg', 1, 'shgf', 'hfd', 800, 1,  1', 1)";
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
