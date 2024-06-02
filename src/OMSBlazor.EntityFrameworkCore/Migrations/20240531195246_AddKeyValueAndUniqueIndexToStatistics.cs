using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddKeyValueAndUniqueIndexToStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "SummaryValue",
                table: "Summaries",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "SummaryName",
                table: "Summaries",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "Sales",
                table: "SalesByEmployees",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SalesByEmployees",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "Sales",
                table: "SalesByCountries",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "SalesByCountries",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "Sales",
                table: "SalesByCategories",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "SalesByCategories",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "Purchases",
                table: "PurchasesByCustomers",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "PurchasesByCustomers",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "ProductsCount",
                table: "ProductsByCategories",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "ProductsByCategories",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "OrdersCount",
                table: "OrdersByCountries",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "OrdersByCountries",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "CustomersCount",
                table: "CustomersByCountries",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "CustomersByCountries",
                newName: "Key");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Summaries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Key",
                table: "SalesByEmployees",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "SalesByEmployees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "SalesByCountries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "SalesByCategories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "PurchasesByCustomers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ProductsByCategories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "OrdersByCountries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "CustomersByCountries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_Key_TenantId",
                table: "Summaries",
                columns: new[] { "Key", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesByEmployees_Key_TenantId",
                table: "SalesByEmployees",
                columns: new[] { "Key", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesByCountries_Key_TenantId",
                table: "SalesByCountries",
                columns: new[] { "Key", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesByCategories_Key_TenantId",
                table: "SalesByCategories",
                columns: new[] { "Key", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesByCustomers_Key_TenantId",
                table: "PurchasesByCustomers",
                columns: new[] { "Key", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsByCategories_Key_TenantId",
                table: "ProductsByCategories",
                columns: new[] { "Key", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersByCountries_Key_TenantId",
                table: "OrdersByCountries",
                columns: new[] { "Key", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomersByCountries_Key_TenantId",
                table: "CustomersByCountries",
                columns: new[] { "Key", "TenantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Summaries_Key_TenantId",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_SalesByEmployees_Key_TenantId",
                table: "SalesByEmployees");

            migrationBuilder.DropIndex(
                name: "IX_SalesByCountries_Key_TenantId",
                table: "SalesByCountries");

            migrationBuilder.DropIndex(
                name: "IX_SalesByCategories_Key_TenantId",
                table: "SalesByCategories");

            migrationBuilder.DropIndex(
                name: "IX_PurchasesByCustomers_Key_TenantId",
                table: "PurchasesByCustomers");

            migrationBuilder.DropIndex(
                name: "IX_ProductsByCategories_Key_TenantId",
                table: "ProductsByCategories");

            migrationBuilder.DropIndex(
                name: "IX_OrdersByCountries_Key_TenantId",
                table: "OrdersByCountries");

            migrationBuilder.DropIndex(
                name: "IX_CustomersByCountries_Key_TenantId",
                table: "CustomersByCountries");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SalesByEmployees");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SalesByCountries");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SalesByCategories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PurchasesByCustomers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductsByCategories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "OrdersByCountries");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CustomersByCountries");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Summaries",
                newName: "SummaryValue");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Summaries",
                newName: "SummaryName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "SalesByEmployees",
                newName: "Sales");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "SalesByEmployees",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "SalesByCountries",
                newName: "Sales");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "SalesByCountries",
                newName: "CountryName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "SalesByCategories",
                newName: "Sales");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "SalesByCategories",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "PurchasesByCustomers",
                newName: "Purchases");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "PurchasesByCustomers",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ProductsByCategories",
                newName: "ProductsCount");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "ProductsByCategories",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "OrdersByCountries",
                newName: "OrdersCount");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "OrdersByCountries",
                newName: "CountryName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "CustomersByCountries",
                newName: "CustomersCount");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "CustomersByCountries",
                newName: "CountryName");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "SalesByEmployees",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Summaries",
                table: "Summaries",
                column: "SummaryName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesByEmployees",
                table: "SalesByEmployees",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesByCountries",
                table: "SalesByCountries",
                column: "CountryName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesByCategories",
                table: "SalesByCategories",
                column: "CategoryName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchasesByCustomers",
                table: "PurchasesByCustomers",
                column: "CompanyName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsByCategories",
                table: "ProductsByCategories",
                column: "CategoryName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersByCountries",
                table: "OrdersByCountries",
                column: "CountryName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersByCountries",
                table: "CustomersByCountries",
                column: "CountryName");
        }
    }
}
