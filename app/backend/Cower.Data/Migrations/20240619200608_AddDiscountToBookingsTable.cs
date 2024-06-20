using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cower.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountToBookingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_discount_applied",
                table: "bookings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_discount_applied",
                table: "bookings");
        }
    }
}
