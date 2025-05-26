using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menu_item",
                columns: new[] { "id", "category", "description", "is_available", "name", "price" },
                values: new object[,]
                {
                    { 4, "MainCourse", "Hearty beef lasagna with mozzarella cheese.", true, "Beef Lasagna", 13.49m },
                    { 5, "MainCourse", "Grilled salmon with lemon butter sauce.", true, "Grilled Salmon", 15.99m },
                    { 6, "Appetizer", "Toasted garlic bread with herbs.", true, "Garlic Bread", 4.99m },
                    { 7, "Appetizer", "Tomatoes, garlic, basil on toasted baguette.", true, "Bruschetta", 5.99m },
                    { 8, "Appetizer", "Mushrooms filled with cheese and herbs.", true, "Stuffed Mushrooms", 6.99m },
                    { 9, "Appetizer", "Vegetable spring rolls with sweet chili sauce.", true, "Spring Rolls", 5.49m },
                    { 10, "Appetizer", "Loaded nachos with cheese and jalapeños.", true, "Nachos", 8.99m },
                    { 11, "Dessert", "Rich chocolate cake with fudge frosting.", true, "Chocolate Cake", 6.99m },
                    { 12, "Dessert", "Creamy cheesecake with a graham cracker crust.", true, "Cheesecake", 6.49m },
                    { 13, "Dessert", "Vanilla ice cream with toppings.", true, "Ice Cream Sundae", 5.99m },
                    { 14, "Dessert", "Italian dessert with coffee and mascarpone.", true, "Tiramisu", 7.49m },
                    { 15, "Dessert", "Fresh seasonal fruit medley.", true, "Fruit Salad", 4.99m },
                    { 16, "Beverage", "Freshly brewed coffee.", true, "Coffee", 2.99m },
                    { 17, "Beverage", "Chilled black tea with lemon.", true, "Iced Tea", 2.99m },
                    { 18, "Beverage", "Blended fruit smoothie with yogurt.", true, "Smoothie", 4.99m },
                    { 19, "Beverage", "Fresh squeezed lemonade.", true, "Lemonade", 3.49m },
                    { 20, "Beverage", "Choice of Coke, Sprite, or Root Beer.", true, "Soda", 2.49m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "menu_item",
                keyColumn: "id",
                keyValue: 20);
        }
    }
}
