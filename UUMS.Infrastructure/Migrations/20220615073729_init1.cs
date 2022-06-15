using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UUMS.Infrastructure.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_FileInfo_AvatarFileId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "AvatarFileId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_User_FileInfo_AvatarFileId",
                table: "User",
                column: "AvatarFileId",
                principalTable: "FileInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_FileInfo_AvatarFileId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "AvatarFileId",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_FileInfo_AvatarFileId",
                table: "User",
                column: "AvatarFileId",
                principalTable: "FileInfo",
                principalColumn: "Id");
        }
    }
}
