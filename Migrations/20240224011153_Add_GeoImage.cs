using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameBrawl.Migrations
{
    /// <inheritdoc />
    public partial class Add_GeoImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "GeolocationImage",
                schema: "dbo",
                table: "UserGeolocations",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeolocationImage",
                schema: "dbo",
                table: "UserGeolocations");
        }
    }
}
