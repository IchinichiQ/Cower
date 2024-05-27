using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cower.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureOnDeleteActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_coworking_floors_images_image_id",
                table: "coworking_floors");

            migrationBuilder.DropForeignKey(
                name: "fk_coworking_seats_images_image_id",
                table: "coworking_seats");

            migrationBuilder.DropIndex(
                name: "ix_images_filename",
                table: "images");
            
            migrationBuilder.DropColumn(
                name: "filename",
                table: "images");

            migrationBuilder.AddForeignKey(
                name: "fk_coworking_floors_images_image_id",
                table: "coworking_floors",
                column: "image_id",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_coworking_seats_images_image_id",
                table: "coworking_seats",
                column: "image_id",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.AddColumn<string>(
                name: "filename",
                table: "images",
                type: "text",
                nullable: false,
                defaultValue: "");

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
    }
}
