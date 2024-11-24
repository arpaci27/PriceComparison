using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceComparison.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_xpath_to_store : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XPathImage",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "XPathName",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "XPathPrice",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XPathImage",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "XPathName",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "XPathPrice",
                table: "Stores");
        }
    }
}
