using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsCommunicate.Infrastructure.Migrations
{
    public partial class permissionFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Groups_GroupId",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_GroupId",
                table: "Permissions",
                newName: "IX_Permissions_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Groups_GroupId",
                table: "Permissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Groups_GroupId",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_GroupId",
                table: "Permission",
                newName: "IX_Permission_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Groups_GroupId",
                table: "Permission",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
