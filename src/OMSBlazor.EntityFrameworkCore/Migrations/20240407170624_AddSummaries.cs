using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddSummaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Summaries",
                columns: table => new
                {
                    SummaryName = table.Column<string>(type: "TEXT", nullable: false),
                    SummaryValue = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summaries", x => x.SummaryName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Summaries");
        }
    }
}
