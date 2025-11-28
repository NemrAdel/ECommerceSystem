using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductAndOrderModule2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_FirsName",
                table: "Order",
                newName: "Address_FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_FirstName",
                table: "Order",
                newName: "Address_FirsName");
        }
    }
}
