using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cower.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueSeatNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_coworking_seats_floor_id",
                table: "coworking_seats");

            migrationBuilder.CreateIndex(
                name: "ix_coworking_seats_floor_id_number",
                table: "coworking_seats",
                columns: new[] { "floor_id", "number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_coworking_seats_floor_id_number",
                table: "coworking_seats");

            migrationBuilder.CreateIndex(
                name: "ix_coworking_seats_floor_id",
                table: "coworking_seats",
                column: "floor_id");
        }
    }
}
