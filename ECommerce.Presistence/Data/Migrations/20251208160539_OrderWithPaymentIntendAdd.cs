using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderWithPaymentIntendAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Order");
        }
    }
}
