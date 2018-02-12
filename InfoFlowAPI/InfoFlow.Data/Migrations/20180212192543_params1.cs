using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InfoFlow.Data.Migrations
{
    public partial class params1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterLinkParams_AspNetUsers_StudentId",
                table: "RegisterLinkParams");

            migrationBuilder.DropIndex(
                name: "IX_RegisterLinkParams_StudentId",
                table: "RegisterLinkParams");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "RegisterLinkParams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "RegisterLinkParams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisterLinkParams_StudentId",
                table: "RegisterLinkParams",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterLinkParams_AspNetUsers_StudentId",
                table: "RegisterLinkParams",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
