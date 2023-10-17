using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsCommunicate.Infrastructure.Migrations
{
    public partial class PermissionEntityFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Groups_GroupId",
                table: "Permissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Groups_GroupId",
                table: "Permissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Groups_GroupId",
                table: "Permissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Groups_GroupId",
                table: "Permissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
