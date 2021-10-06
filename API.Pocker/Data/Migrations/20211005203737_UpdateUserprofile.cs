using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Pocker.Data.Migrations
{
    public partial class UpdateUserprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                table: "tb_userHistory",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_userHistory_UserProfileId",
                table: "tb_userHistory",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_userHistory_tb_userProfile_UserProfileId",
                table: "tb_userHistory",
                column: "UserProfileId",
                principalTable: "tb_userProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_userHistory_tb_userProfile_UserProfileId",
                table: "tb_userHistory");

            migrationBuilder.DropIndex(
                name: "IX_tb_userHistory_UserProfileId",
                table: "tb_userHistory");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "tb_userHistory");
        }
    }
}
