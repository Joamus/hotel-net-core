using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBackendApi.Migrations
{
    /// <inheritdoc />
    public partial class RoomReservationsApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "RoomReservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "RoomReservations");
        }
    }
}
