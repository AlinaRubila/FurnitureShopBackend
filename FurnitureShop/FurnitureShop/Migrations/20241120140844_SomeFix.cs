using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureShop.Migrations
{
    /// <inheritdoc />
    public partial class SomeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemUser_Items_CartsID",
                table: "ItemUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemUser_Users_CartsID1",
                table: "ItemUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemUser",
                table: "ItemUser");

            migrationBuilder.RenameTable(
                name: "ItemUser",
                newName: "Carts");

            migrationBuilder.RenameColumn(
                name: "CartsID1",
                table: "Carts",
                newName: "UsersID");

            migrationBuilder.RenameColumn(
                name: "CartsID",
                table: "Carts",
                newName: "ItemsID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemUser_CartsID1",
                table: "Carts",
                newName: "IX_Carts_UsersID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                columns: new[] { "ItemsID", "UsersID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Items_ItemsID",
                table: "Carts",
                column: "ItemsID",
                principalTable: "Items",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UsersID",
                table: "Carts",
                column: "UsersID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Items_ItemsID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UsersID",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "ItemUser");

            migrationBuilder.RenameColumn(
                name: "UsersID",
                table: "ItemUser",
                newName: "CartsID1");

            migrationBuilder.RenameColumn(
                name: "ItemsID",
                table: "ItemUser",
                newName: "CartsID");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_UsersID",
                table: "ItemUser",
                newName: "IX_ItemUser_CartsID1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemUser",
                table: "ItemUser",
                columns: new[] { "CartsID", "CartsID1" });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemUser_Items_CartsID",
                table: "ItemUser",
                column: "CartsID",
                principalTable: "Items",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemUser_Users_CartsID1",
                table: "ItemUser",
                column: "CartsID1",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
