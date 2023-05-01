using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestSignalR.Migrations
{
    public partial class d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "UserName");
        }
    }
}
