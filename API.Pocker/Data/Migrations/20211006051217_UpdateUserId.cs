using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Pocker.Data.Migrations
{
    public partial class UpdateUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdUserIdentity",
                table: "tb_userProfile",
                newName: "UserIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_userProfile_UserIdentityId",
                table: "tb_userProfile",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_userProfile_AspNetUsers_UserIdentityId",
                table: "tb_userProfile",
                column: "UserIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_userProfile_AspNetUsers_UserIdentityId",
                table: "tb_userProfile");

            migrationBuilder.DropIndex(
                name: "IX_tb_userProfile_UserIdentityId",
                table: "tb_userProfile");

            migrationBuilder.RenameColumn(
                name: "UserIdentityId",
                table: "tb_userProfile",
                newName: "IdUserIdentity");
        }
    }
}
