using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeReservationNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_reservation_reservation_id",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "reservation_id",
                table: "Orders",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_reservation_reservation_id",
                table: "Orders",
                column: "reservation_id",
                principalTable: "reservation",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_reservation_reservation_id",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "reservation_id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_reservation_reservation_id",
                table: "Orders",
                column: "reservation_id",
                principalTable: "reservation",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
