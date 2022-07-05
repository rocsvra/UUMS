using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UUMS.Infrastructure.Migrations
{
    public partial class role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Role",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Role_ClientId",
                table: "Role",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Client_ClientId",
                table: "Role",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Client_ClientId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_ClientId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Role");
        }
    }
}
