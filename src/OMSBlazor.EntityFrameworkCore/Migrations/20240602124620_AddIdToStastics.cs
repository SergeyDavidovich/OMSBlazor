using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToStastics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Summaries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SalesByEmployees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SalesByCountries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SalesByCategories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PurchasesByCustomers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductsByCategories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrdersByCountries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CustomersByCountries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Summaries",
                table: "Summaries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesByEmployees",
                table: "SalesByEmployees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesByCountries",
                table: "SalesByCountries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesByCategories",
                table: "SalesByCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchasesByCustomers",
                table: "PurchasesByCustomers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsByCategories",
                table: "ProductsByCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersByCountries",
                table: "OrdersByCountries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersByCountries",
                table: "CustomersByCountries",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Summaries",
                table: "Summaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesByEmployees",
                table: "SalesByEmployees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesByCountries",
                table: "SalesByCountries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesByCategories",
                table: "SalesByCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchasesByCustomers",
                table: "PurchasesByCustomers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsByCategories",
                table: "ProductsByCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersByCountries",
                table: "OrdersByCountries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomersByCountries",
                table: "CustomersByCountries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SalesByEmployees");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SalesByCountries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SalesByCategories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PurchasesByCustomers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductsByCategories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrdersByCountries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomersByCountries");
        }
    }
}
