using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameBrawl.Migrations
{
    /// <inheritdoc />
    public partial class MatchPanel_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGeolocations_UserId",
                schema: "dbo",
                table: "UserGeolocations");

            migrationBuilder.AddColumn<string>(
                name: "MatchHostId",
                schema: "dbo",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchProgress",
                schema: "dbo",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserGeolocations_UserId",
                schema: "dbo",
                table: "UserGeolocations",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchHostId",
                schema: "dbo",
                table: "Matches",
                column: "MatchHostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_MatchHostId",
                schema: "dbo",
                table: "Matches",
                column: "MatchHostId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_MatchHostId",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_UserGeolocations_UserId",
                schema: "dbo",
                table: "UserGeolocations");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MatchHostId",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchHostId",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchProgress",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_UserGeolocations_UserId",
                schema: "dbo",
                table: "UserGeolocations",
                column: "UserId");
        }
    }
}
