using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdminUserseedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "dietary_notes", "email", "first_name", "last_name", "password_hash", "password_salt", "phone", "rewards_points", "role" },
                values: new object[] { 2, null, "admin@gmail.com", "Admin", "Test", "+1fzEaaKIt+hxB8eZ5RK6sywJXqY5Qkn7CxNoG6ckxc=", "hHA0qDG/iJSbc9PEUXJ8UQ==", "111-111-1111", 0, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
