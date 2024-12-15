using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foodDB.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "MenuItemItemID",
                table: "OrderItems",
                newName: "MenuItemID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_MenuItemItemID",
                table: "OrderItems",
                newName: "IX_OrderItems_MenuItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemID",
                table: "OrderItems",
                column: "MenuItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemID",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "MenuItemID",
                table: "OrderItems",
                newName: "MenuItemItemID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_MenuItemID",
                table: "OrderItems",
                newName: "IX_OrderItems_MenuItemItemID");

            migrationBuilder.AddColumn<int>(
                name: "ItemID",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems",
                column: "MenuItemItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
