﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cower.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "booking_date",
                table: "bookings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "booking_date",
                table: "bookings");
        }
    }
}