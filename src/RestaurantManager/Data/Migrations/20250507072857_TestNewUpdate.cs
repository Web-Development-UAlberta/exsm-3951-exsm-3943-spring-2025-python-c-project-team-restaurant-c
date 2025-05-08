using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class TestNewUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_TagId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_menu_item_MenuItemId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_order_menu_item_menu_item_MenuItemId",
                table: "order_menu_item");

            migrationBuilder.DropForeignKey(
                name: "FK_order_menu_item_order_OrderId",
                table: "order_menu_item");

            migrationBuilder.DropForeignKey(
                name: "FK_user_dietary_tag_dietary_tag_TagId",
                table: "user_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_user_dietary_tag_user_UserId",
                table: "user_dietary_tag");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "user_dietary_tag",
                newName: "tag_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_dietary_tag",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_dietary_tag_TagId",
                table: "user_dietary_tag",
                newName: "IX_user_dietary_tag_tag_id");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "order_menu_item",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "order_menu_item",
                newName: "menu_item_id");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "order_menu_item",
                newName: "order_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_menu_item_MenuItemId",
                table: "order_menu_item",
                newName: "IX_order_menu_item_menu_item_id");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "order",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "menu_item_dietary_tag",
                newName: "tag_id");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "menu_item_dietary_tag",
                newName: "menu_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_menu_item_dietary_tag_TagId",
                table: "menu_item_dietary_tag",
                newName: "IX_menu_item_dietary_tag_tag_id");

            migrationBuilder.AlterColumn<int>(
                name: "tag_id",
                table: "user_dietary_tag",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "user_dietary_tag",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "province",
                table: "user_address",
                type: "VARCHAR(100)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "postal_code",
                table: "user_address",
                type: "VARCHAR(20)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "user_address",
                type: "VARCHAR(100)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "user_address",
                type: "VARCHAR(100)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "address_line_2",
                table: "user_address",
                type: "VARCHAR(255)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address_line_1",
                table: "user_address",
                type: "VARCHAR(255)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "user",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "user",
                type: "VARCHAR(50)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "password_salt",
                table: "user",
                type: "VARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "user",
                type: "VARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "user",
                type: "VARCHAR(255)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "user",
                type: "VARCHAR(255)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user",
                type: "VARCHAR(255)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "table_number",
                table: "reservation",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "reservation",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "menu_item_id",
                table: "order_menu_item",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "order_id",
                table: "order_menu_item",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "tip_amount",
                table: "order",
                type: "REAL",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax",
                table: "order",
                type: "REAL",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "order",
                type: "REAL",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "order",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "order_type",
                table: "order",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "delivery_fee",
                table: "order",
                type: "REAL",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "order",
                type: "REAL",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "menu_item",
                type: "REAL",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "menu_item",
                type: "VARCHAR(255)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "menu_item",
                type: "VARCHAR(255)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "menu_item",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "dietary_tag",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_tag_id",
                table: "menu_item_dietary_tag",
                column: "tag_id",
                principalTable: "dietary_tag",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_dietary_tag_menu_item_menu_item_id",
                table: "menu_item_dietary_tag",
                column: "menu_item_id",
                principalTable: "menu_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_menu_item_menu_item_menu_item_id",
                table: "order_menu_item",
                column: "menu_item_id",
                principalTable: "menu_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_menu_item_order_order_id",
                table: "order_menu_item",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_dietary_tag_dietary_tag_tag_id",
                table: "user_dietary_tag",
                column: "tag_id",
                principalTable: "dietary_tag",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_dietary_tag_user_user_id",
                table: "user_dietary_tag",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_tag_id",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_menu_item_menu_item_id",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_order_menu_item_menu_item_menu_item_id",
                table: "order_menu_item");

            migrationBuilder.DropForeignKey(
                name: "FK_order_menu_item_order_order_id",
                table: "order_menu_item");

            migrationBuilder.DropForeignKey(
                name: "FK_user_dietary_tag_dietary_tag_tag_id",
                table: "user_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_user_dietary_tag_user_user_id",
                table: "user_dietary_tag");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "user_dietary_tag",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "user_dietary_tag",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_user_dietary_tag_tag_id",
                table: "user_dietary_tag",
                newName: "IX_user_dietary_tag_TagId");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "order_menu_item",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "menu_item_id",
                table: "order_menu_item",
                newName: "MenuItemId");

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "order_menu_item",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_order_menu_item_menu_item_id",
                table: "order_menu_item",
                newName: "IX_order_menu_item_MenuItemId");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "order",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "menu_item_dietary_tag",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "menu_item_id",
                table: "menu_item_dietary_tag",
                newName: "MenuItemId");

            migrationBuilder.RenameIndex(
                name: "IX_menu_item_dietary_tag_tag_id",
                table: "menu_item_dietary_tag",
                newName: "IX_menu_item_dietary_tag_TagId");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "user_dietary_tag",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "user_dietary_tag",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "province",
                table: "user_address",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "postal_code",
                table: "user_address",
                type: "TEXT",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "user_address",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "user_address",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "address_line_2",
                table: "user_address",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address_line_1",
                table: "user_address",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "user",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "user",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "password_salt",
                table: "user",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "user",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "user",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "user",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "table_number",
                table: "reservation",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "reservation",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemId",
                table: "order_menu_item",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "order_menu_item",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "tip_amount",
                table: "order",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "tax",
                table: "order",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "order",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "order",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "order_type",
                table: "order",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<decimal>(
                name: "delivery_fee",
                table: "order",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "REAL",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "total",
                table: "order",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "menu_item",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "menu_item",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "menu_item",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "menu_item",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "dietary_tag",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_TagId",
                table: "menu_item_dietary_tag",
                column: "TagId",
                principalTable: "dietary_tag",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_dietary_tag_menu_item_MenuItemId",
                table: "menu_item_dietary_tag",
                column: "MenuItemId",
                principalTable: "menu_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_menu_item_menu_item_MenuItemId",
                table: "order_menu_item",
                column: "MenuItemId",
                principalTable: "menu_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_menu_item_order_OrderId",
                table: "order_menu_item",
                column: "OrderId",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_dietary_tag_dietary_tag_TagId",
                table: "user_dietary_tag",
                column: "TagId",
                principalTable: "dietary_tag",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_dietary_tag_user_UserId",
                table: "user_dietary_tag",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
