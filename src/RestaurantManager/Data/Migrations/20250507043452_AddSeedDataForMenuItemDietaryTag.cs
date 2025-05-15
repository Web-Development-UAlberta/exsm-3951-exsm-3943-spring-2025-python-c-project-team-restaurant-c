using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForMenuItemDietaryTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_DietaryTagId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropIndex(
                name: "IX_menu_item_dietary_tag_DietaryTagId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropColumn(
                name: "DietaryTagId",
                table: "menu_item_dietary_tag");

            migrationBuilder.CreateIndex(
                name: "IX_menu_item_dietary_tag_TagId",
                table: "menu_item_dietary_tag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_TagId",
                table: "menu_item_dietary_tag",
                column: "TagId",
                principalTable: "dietary_tag",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menu_item_dietary_tag_dietary_tag_TagId",
                table: "menu_item_dietary_tag");

            migrationBuilder.DropIndex(
                name: "IX_menu_item_dietary_tag_TagId",
                table: "menu_item_dietary_tag");

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
        }
    }
}
