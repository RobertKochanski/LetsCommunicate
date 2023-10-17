using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsCommunicate.Infrastructure.Migrations
{
    public partial class changedGroupMembersPropName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserGroup_AspNetUsers_AppUsersId",
                table: "AppUserGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserGroup",
                table: "AppUserGroup");

            migrationBuilder.DropIndex(
                name: "IX_AppUserGroup_GroupsId",
                table: "AppUserGroup");

            migrationBuilder.RenameColumn(
                name: "AppUsersId",
                table: "AppUserGroup",
                newName: "MembersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserGroup",
                table: "AppUserGroup",
                columns: new[] { "GroupsId", "MembersId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserGroup_MembersId",
                table: "AppUserGroup",
                column: "MembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserGroup_AspNetUsers_MembersId",
                table: "AppUserGroup",
                column: "MembersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserGroup_AspNetUsers_MembersId",
                table: "AppUserGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserGroup",
                table: "AppUserGroup");

            migrationBuilder.DropIndex(
                name: "IX_AppUserGroup_MembersId",
                table: "AppUserGroup");

            migrationBuilder.RenameColumn(
                name: "MembersId",
                table: "AppUserGroup",
                newName: "AppUsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserGroup",
                table: "AppUserGroup",
                columns: new[] { "AppUsersId", "GroupsId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserGroup_GroupsId",
                table: "AppUserGroup",
                column: "GroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserGroup_AspNetUsers_AppUsersId",
                table: "AppUserGroup",
                column: "AppUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
