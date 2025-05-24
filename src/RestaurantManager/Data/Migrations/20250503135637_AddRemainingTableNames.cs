using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemDietaryTags_DietaryTags_DietaryTagId",
                table: "MenuItemDietaryTags");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemDietaryTags_MenuItems_MenuItemId",
                table: "MenuItemDietaryTags");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMenuItems_MenuItems_MenuItemId",
                table: "OrderMenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMenuItems_Orders_OrderId",
                table: "OrderMenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_reservation_reservation_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_user_address_address_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_user_user_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDietaryTags_DietaryTags_TagId",
                table: "UserDietaryTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDietaryTags_user_UserId",
                table: "UserDietaryTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDietaryTags",
                table: "UserDietaryTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderMenuItems",
                table: "OrderMenuItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemDietaryTags",
                table: "MenuItemDietaryTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DietaryTags",
                table: "DietaryTags");

            migrationBuilder.RenameTable(
                name: "UserDietaryTags",
                newName: "user_dietary_tag");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "order");

            migrationBuilder.RenameTable(
                name: "OrderMenuItems",
                newName: "order_menu_item");

            migrationBuilder.RenameTable(
                name: "MenuItems",
                newName: "menu_item");

            migrationBuilder.RenameTable(
                name: "MenuItemDietaryTags",
                newName: "menu_item_dietary_tag");

            migrationBuilder.RenameTable(
                name: "DietaryTags",
                newName: "dietary_tag");

            migrationBuilder.RenameIndex(
                name: "IX_UserDietaryTags_TagId",
                table: "user_dietary_tag",
                newName: "IX_user_dietary_tag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_user_id",
                table: "order",
                newName: "IX_order_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_reservation_id",
                table: "order",
                newName: "IX_order_reservation_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_address_id",
                table: "order",
                newName: "IX_order_address_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderMenuItems_MenuItemId",
                table: "order_menu_item",
                newName: "IX_order_menu_item_MenuItemId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItemDietaryTags_DietaryTagId",
                table: "menu_item_dietary_tag",
                newName: "IX_menu_item_dietary_tag_DietaryTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_dietary_tag",
                table: "user_dietary_tag",
                columns: new[] { "UserId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_order",
                table: "order",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_menu_item",
                table: "order_menu_item",
                columns: new[] { "OrderId", "MenuItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_menu_item",
                table: "menu_item",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_menu_item_dietary_tag",
                table: "menu_item_dietary_tag",
                columns: new[] { "MenuItemId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_dietary_tag",
                table: "dietary_tag",
                column: "id");

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
                name: "FK_order_reservation_reservation_id",
                table: "order",
                column: "reservation_id",
                principalTable: "reservation",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_user_address_address_id",
                table: "order",
                column: "address_id",
                principalTable: "user_address",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_user_user_id",
                table: "order",
                column: "user_id",
                principalTable: "user",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_DietaryTagId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_menu_item_MenuItemId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_order_reservation_reservation_id",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_order_user_address_address_id",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_order_user_user_id",
                table: "order");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_dietary_tag",
                table: "user_dietary_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_menu_item",
                table: "order_menu_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_menu_item_dietary_tag",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_menu_item",
                table: "menu_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dietary_tag",
                table: "dietary_tag");

            migrationBuilder.RenameTable(
                name: "user_dietary_tag",
                newName: "UserDietaryTags");

            migrationBuilder.RenameTable(
                name: "order_menu_item",
                newName: "OrderMenuItems");

            migrationBuilder.RenameTable(
                name: "order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "menu_item_dietary_tag",
                newName: "MenuItemDietaryTags");

            migrationBuilder.RenameTable(
                name: "menu_item",
                newName: "MenuItems");

            migrationBuilder.RenameTable(
                name: "dietary_tag",
                newName: "DietaryTags");

            migrationBuilder.RenameIndex(
                name: "IX_user_dietary_tag_TagId",
                table: "UserDietaryTags",
                newName: "IX_UserDietaryTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_order_menu_item_MenuItemId",
                table: "OrderMenuItems",
                newName: "IX_OrderMenuItems_MenuItemId");

            migrationBuilder.RenameIndex(
                name: "IX_order_user_id",
                table: "Orders",
                newName: "IX_Orders_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_reservation_id",
                table: "Orders",
                newName: "IX_Orders_reservation_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_address_id",
                table: "Orders",
                newName: "IX_Orders_address_id");

            migrationBuilder.RenameIndex(
                name: "IX_menu_item_dietary_tag_DietaryTagId",
                table: "MenuItemDietaryTags",
                newName: "IX_MenuItemDietaryTags_DietaryTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDietaryTags",
                table: "UserDietaryTags",
                columns: new[] { "UserId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderMenuItems",
                table: "OrderMenuItems",
                columns: new[] { "OrderId", "MenuItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemDietaryTags",
                table: "MenuItemDietaryTags",
                columns: new[] { "MenuItemId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DietaryTags",
                table: "DietaryTags",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemDietaryTags_DietaryTags_DietaryTagId",
                table: "MenuItemDietaryTags",
                column: "DietaryTagId",
                principalTable: "DietaryTags",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemDietaryTags_MenuItems_MenuItemId",
                table: "MenuItemDietaryTags",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderMenuItems_MenuItems_MenuItemId",
                table: "OrderMenuItems",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderMenuItems_Orders_OrderId",
                table: "OrderMenuItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_reservation_reservation_id",
                table: "Orders",
                column: "reservation_id",
                principalTable: "reservation",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_user_address_address_id",
                table: "Orders",
                column: "address_id",
                principalTable: "user_address",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_user_user_id",
                table: "Orders",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDietaryTags_DietaryTags_TagId",
                table: "UserDietaryTags",
                column: "TagId",
                principalTable: "DietaryTags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDietaryTags_user_UserId",
                table: "UserDietaryTags",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
