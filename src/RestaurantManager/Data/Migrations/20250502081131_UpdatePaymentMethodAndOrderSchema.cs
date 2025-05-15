using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentMethodAndOrderSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "card_number",
                table: "payment_method");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "Orders",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Orders",
                newName: "tax");

            migrationBuilder.AddColumn<string>(
                name: "payment_processor_token",
                table: "payment_method",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "subtotal",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_processor_token",
                table: "payment_method");

            migrationBuilder.DropColumn(
                name: "subtotal",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "Orders",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "tax",
                table: "Orders",
                newName: "price");

            migrationBuilder.AddColumn<string>(
                name: "card_number",
                table: "payment_method",
                type: "TEXT",
                nullable: true);
        }
    }
}
