using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Pocker.Data.Migrations
{
    public partial class Add_in_tb_refreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "tb_refreshToken",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "tb_refreshToken",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "tb_refreshToken");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "tb_refreshToken");
        }
    }
}
