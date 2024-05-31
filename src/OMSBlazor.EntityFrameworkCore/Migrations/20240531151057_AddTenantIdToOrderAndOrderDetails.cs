using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdToOrderAndOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "OrderDetails",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "OrderDetails");
        }
    }
}
