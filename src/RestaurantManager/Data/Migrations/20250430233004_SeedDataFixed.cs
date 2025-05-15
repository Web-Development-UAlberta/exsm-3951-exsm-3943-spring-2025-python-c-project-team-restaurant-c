using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemDietaryTags_DietaryTags_TagId",
                table: "MenuItemDietaryTags");

            migrationBuilder.DropIndex(
                name: "IX_MenuItemDietaryTags_TagId",
                table: "MenuItemDietaryTags");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "MenuItemDietaryTags",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemId",
                table: "MenuItemDietaryTags",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "DietaryTagId",
                table: "MenuItemDietaryTags",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.InsertData(
                table: "DietaryTags",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Vegan" },
                    { 2, "Gluten-Free" },
                    { 3, "Dairy-Free" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "id", "category", "description", "is_available", "name", "price" },
                values: new object[,]
                {
                    { 1, null, "A delicious vegan pizza with gluten-free crust.", false, "Vegan Pizza", 12.99m },
                    { 2, null, "A tasty chicken wrap with fresh vegetables.", false, "Chicken Wrap", 9.99m },
                    { 3, null, "A classic Caesar salad with creamy dressing.", false, "Caesar Salad", 7.99m }
                });

            migrationBuilder.InsertData(
                table: "MenuItemDietaryTags",
                columns: new[] { "MenuItemId", "TagId", "DietaryTagId" },
                values: new object[,]
                {
                    { 1, 1, null },
                    { 1, 2, null },
                    { 2, 3, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemDietaryTags_DietaryTagId",
                table: "MenuItemDietaryTags",
                column: "DietaryTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemDietaryTags_DietaryTags_DietaryTagId",
                table: "MenuItemDietaryTags",
                column: "DietaryTagId",
                principalTable: "DietaryTags",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemDietaryTags_DietaryTags_DietaryTagId",
                table: "MenuItemDietaryTags");

            migrationBuilder.DropIndex(
                name: "IX_MenuItemDietaryTags_DietaryTagId",
                table: "MenuItemDietaryTags");

            migrationBuilder.DeleteData(
                table: "DietaryTags",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DietaryTags",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DietaryTags",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItemDietaryTags",
                keyColumns: new[] { "MenuItemId", "TagId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "MenuItemDietaryTags",
                keyColumns: new[] { "MenuItemId", "TagId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "MenuItemDietaryTags",
                keyColumns: new[] { "MenuItemId", "TagId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "DietaryTagId",
                table: "MenuItemDietaryTags");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "MenuItemDietaryTags",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemId",
                table: "MenuItemDietaryTags",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemDietaryTags_TagId",
                table: "MenuItemDietaryTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemDietaryTags_DietaryTags_TagId",
                table: "MenuItemDietaryTags",
                column: "TagId",
                principalTable: "DietaryTags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
