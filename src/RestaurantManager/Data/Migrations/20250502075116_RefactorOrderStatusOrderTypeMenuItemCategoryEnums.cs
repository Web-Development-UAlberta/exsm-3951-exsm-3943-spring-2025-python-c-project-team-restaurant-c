using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorOrderStatusOrderTypeMenuItemCategoryEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "order_type",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "MenuItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 1,
                column: "category",
                value: "Appetizer");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 2,
                column: "category",
                value: "Appetizer");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 3,
                column: "category",
                value: "Appetizer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "order_type",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "MenuItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 1,
                column: "category",
                value: null);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 2,
                column: "category",
                value: null);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 3,
                column: "category",
                value: null);
        }
    }
}
