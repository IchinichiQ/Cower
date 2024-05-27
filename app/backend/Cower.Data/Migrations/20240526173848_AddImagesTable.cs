using Cower.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cower.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_coworking_seats_coworkings_coworking_id",
                table: "coworking_seats");

            migrationBuilder.DropColumn(
                name: "image_filename",
                table: "coworking_seats");

            migrationBuilder.RenameColumn(
                name: "coworking_id",
                table: "coworking_seats",
                newName: "image_id");

            migrationBuilder.RenameIndex(
                name: "ix_coworking_seats_coworking_id",
                table: "coworking_seats",
                newName: "ix_coworking_seats_image_id");

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<ImageType>(type: "image_type", nullable: false),
                    extension = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    filename = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_coworking_floors_image_id",
                table: "coworking_floors",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "ix_images_filename",
                table: "images",
                column: "filename",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_coworking_floors_images_image_id",
                table: "coworking_floors",
                column: "image_id",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_coworking_seats_images_image_id",
                table: "coworking_seats",
                column: "image_id",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_coworking_floors_images_image_id",
                table: "coworking_floors");

            migrationBuilder.DropForeignKey(
                name: "fk_coworking_seats_images_image_id",
                table: "coworking_seats");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropIndex(
                name: "ix_coworking_floors_image_id",
                table: "coworking_floors");

            migrationBuilder.RenameColumn(
                name: "image_id",
                table: "coworking_seats",
                newName: "coworking_id");

            migrationBuilder.RenameIndex(
                name: "ix_coworking_seats_image_id",
                table: "coworking_seats",
                newName: "ix_coworking_seats_coworking_id");

            migrationBuilder.AddColumn<string>(
                name: "image_filename",
                table: "coworking_seats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "fk_coworking_seats_coworkings_coworking_id",
                table: "coworking_seats",
                column: "coworking_id",
                principalTable: "coworkings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
