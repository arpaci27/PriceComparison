using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceComparison.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_xpathPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XPathPrice",
                table: "StoreCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XPathPrice",
                table: "StoreCategories");
        }
    }
}
