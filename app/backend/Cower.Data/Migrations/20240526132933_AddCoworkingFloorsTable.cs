using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cower.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCoworkingFloorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coworking_floors_media");

            migrationBuilder.DropColumn(
                name: "floors",
                table: "coworkings");

            migrationBuilder.DropColumn(
                name: "floor",
                table: "coworking_seats");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:booking_status", "awaiting_payment,paid,in_progress,success,cancelled,payment_timeout")
                .Annotation("Npgsql:Enum:image_type", "floor,seat")
                .OldAnnotation("Npgsql:Enum:booking_status", "awaiting_payment,paid,in_progress,success,cancelled,payment_timeout");

            migrationBuilder.AddColumn<long>(
                name: "floor_id",
                table: "coworking_seats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "coworking_floors",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coworking_id = table.Column<long>(type: "bigint", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    image_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coworking_floors", x => x.id);
                    table.ForeignKey(
                        name: "fk_coworking_floors_coworkings_coworking_id",
                        column: x => x.coworking_id,
                        principalTable: "coworkings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_coworking_seats_floor_id",
                table: "coworking_seats",
                column: "floor_id");

            migrationBuilder.CreateIndex(
                name: "ix_coworking_floors_coworking_id_number",
                table: "coworking_floors",
                columns: new[] { "coworking_id", "number" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_coworking_seats_coworking_floors_floor_id",
                table: "coworking_seats",
                column: "floor_id",
                principalTable: "coworking_floors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_coworking_seats_coworking_floors_floor_id",
                table: "coworking_seats");

            migrationBuilder.DropTable(
                name: "coworking_floors");

            migrationBuilder.DropIndex(
                name: "ix_coworking_seats_floor_id",
                table: "coworking_seats");

            migrationBuilder.DropColumn(
                name: "floor_id",
                table: "coworking_seats");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:booking_status", "awaiting_payment,paid,in_progress,success,cancelled,payment_timeout")
                .OldAnnotation("Npgsql:Enum:booking_status", "awaiting_payment,paid,in_progress,success,cancelled,payment_timeout")
                .OldAnnotation("Npgsql:Enum:image_type", "floor,seat");

            migrationBuilder.AddColumn<int>(
                name: "floors",
                table: "coworkings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "floor",
                table: "coworking_seats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "coworking_floors_media",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coworking_id = table.Column<long>(type: "bigint", nullable: false),
                    background_filename = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coworking_floors_media", x => x.id);
                    table.ForeignKey(
                        name: "fk_coworking_floors_media_coworkings_coworking_id",
                        column: x => x.coworking_id,
                        principalTable: "coworkings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_coworking_floors_media_coworking_id",
                table: "coworking_floors_media",
                column: "coworking_id");
        }
    }
}
