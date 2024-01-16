using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSBlazor.OMSBlazorIdentity
{
    /// <inheritdoc />
    public partial class AddLastPasswordChangeTimeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastPasswordChangeTime",
                table: "AbpUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldChangePasswordOnNextLogin",
                table: "AbpUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPasswordChangeTime",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "ShouldChangePasswordOnNextLogin",
                table: "AbpUsers");
        }
    }
}
