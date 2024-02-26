using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameBrawl.Migrations
{
    /// <inheritdoc />
    public partial class MatchModel_Locationdata_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location_Latitude",
                schema: "dbo",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location_Longitude",
                schema: "dbo",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Location_Longitude",
                schema: "dbo",
                table: "Matches");
        }
    }
}
