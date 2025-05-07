using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDataTypesManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_DietaryTagId",
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

            migrationBuilder.DropIndex(
                name: "IX_menu_item_dietary_tag_DietaryTagId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropColumn(
                name: "DietaryTagId",
                table: "menu_item_dietary_tag");

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
                name: "TagId",
                table: "menu_item_dietary_tag",
                newName: "tag_id");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "menu_item_dietary_tag",
                newName: "menu_item_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_menu_item_dietary_tag_tag_id",
                table: "menu_item_dietary_tag",
                column: "tag_id");

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

            migrationBuilder.DropIndex(
                name: "IX_menu_item_dietary_tag_tag_id",
                table: "menu_item_dietary_tag");

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
                name: "tag_id",
                table: "menu_item_dietary_tag",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "menu_item_id",
                table: "menu_item_dietary_tag",
                newName: "MenuItemId");

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

            migrationBuilder.AddColumn<int>(
                name: "DietaryTagId",
                table: "menu_item_dietary_tag",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "menu_item_dietary_tag",
                keyColumns: new[] { "MenuItemId", "TagId" },
                keyValues: new object[] { 1, 1 },
                column: "DietaryTagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "menu_item_dietary_tag",
                keyColumns: new[] { "MenuItemId", "TagId" },
                keyValues: new object[] { 1, 2 },
                column: "DietaryTagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "menu_item_dietary_tag",
                keyColumns: new[] { "MenuItemId", "TagId" },
                keyValues: new object[] { 2, 3 },
                column: "DietaryTagId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_menu_item_dietary_tag_DietaryTagId",
                table: "menu_item_dietary_tag",
                column: "DietaryTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_DietaryTagId",
                table: "menu_item_dietary_tag",
                column: "DietaryTagId",
                principalTable: "dietary_tag",
                principalColumn: "id");

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
