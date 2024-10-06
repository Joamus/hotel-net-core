using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBackendApi.Migrations
{
    /// <inheritdoc />
    public partial class RoomReservationUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RoomReservations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RoomReservations");
        }
    }
}
