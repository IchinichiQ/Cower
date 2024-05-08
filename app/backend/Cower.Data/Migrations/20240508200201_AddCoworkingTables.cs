using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cower.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCoworkingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "coworkings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(type: "text", nullable: false),
                    floors = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coworkings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coworking_floors_media",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<int>(type: "integer", nullable: false),
                    background_filename = table.Column<string>(type: "text", nullable: false),
                    coworking_id = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "coworking_seats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    floor = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    image_filename = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    coworking_id = table.Column<long>(type: "bigint", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    x = table.Column<int>(type: "integer", nullable: false),
                    y = table.Column<int>(type: "integer", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    angle = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coworking_seats", x => x.id);
                    table.ForeignKey(
                        name: "fk_coworking_seats_coworkings_coworking_id",
                        column: x => x.coworking_id,
                        principalTable: "coworkings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coworkings_working_time",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    open = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    close = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    coworking_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coworkings_working_time", x => x.id);
                    table.ForeignKey(
                        name: "fk_coworkings_working_time_coworkings_coworking_id",
                        column: x => x.coworking_id,
                        principalTable: "coworkings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    seat_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    seat_number = table.Column<int>(type: "integer", nullable: false),
                    floor = table.Column<int>(type: "integer", nullable: false),
                    coworking_address = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookings", x => x.id);
                    table.ForeignKey(
                        name: "fk_bookings_coworking_seats_seat_id",
                        column: x => x.seat_id,
                        principalTable: "coworking_seats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bookings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bookings_seat_id",
                table: "bookings",
                column: "seat_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_user_id",
                table: "bookings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_coworking_floors_media_coworking_id",
                table: "coworking_floors_media",
                column: "coworking_id");

            migrationBuilder.CreateIndex(
                name: "ix_coworking_seats_coworking_id",
                table: "coworking_seats",
                column: "coworking_id");

            migrationBuilder.CreateIndex(
                name: "ix_coworkings_working_time_coworking_id",
                table: "coworkings_working_time",
                column: "coworking_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "coworking_floors_media");

            migrationBuilder.DropTable(
                name: "coworkings_working_time");

            migrationBuilder.DropTable(
                name: "coworking_seats");

            migrationBuilder.DropTable(
                name: "coworkings");
        }
    }
}
