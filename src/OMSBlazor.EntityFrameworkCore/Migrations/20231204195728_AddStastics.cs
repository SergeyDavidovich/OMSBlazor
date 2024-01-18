using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddStastics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomersByCountries",
                columns: table => new
                {
                    CountryName = table.Column<string>(type: "TEXT", nullable: false),
                    CustomersCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersByCountries", x => x.CountryName);
                });

            migrationBuilder.CreateTable(
                name: "OrdersByCountries",
                columns: table => new
                {
                    CountryName = table.Column<string>(type: "TEXT", nullable: false),
                    OrdersCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersByCountries", x => x.CountryName);
                });

            migrationBuilder.CreateTable(
                name: "ProductsByCategories",
                columns: table => new
                {
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    ProductsCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsByCategories", x => x.CategoryName);
                });

            migrationBuilder.CreateTable(
                name: "PurchasesByCustomers",
                columns: table => new
                {
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    Purchases = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasesByCustomers", x => x.CompanyName);
                });

            migrationBuilder.CreateTable(
                name: "SalesByCategories",
                columns: table => new
                {
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    Sales = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesByCategories", x => x.CategoryName);
                });

            migrationBuilder.CreateTable(
                name: "SalesByCountries",
                columns: table => new
                {
                    CountryName = table.Column<string>(type: "TEXT", nullable: false),
                    Sales = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesByCountries", x => x.CountryName);
                });

            migrationBuilder.CreateTable(
                name: "SalesByEmployees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sales = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesByEmployees", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersByCountries");

            migrationBuilder.DropTable(
                name: "OrdersByCountries");

            migrationBuilder.DropTable(
                name: "ProductsByCategories");

            migrationBuilder.DropTable(
                name: "PurchasesByCustomers");

            migrationBuilder.DropTable(
                name: "SalesByCategories");

            migrationBuilder.DropTable(
                name: "SalesByCountries");

            migrationBuilder.DropTable(
                name: "SalesByEmployees");
        }
    }
}
