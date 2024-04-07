using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddLastNameColumnToSalesByEmployeeStat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "SalesByEmployees",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesByEmployees",
                table: "LastName");
        }
    }
}
