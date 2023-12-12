using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Migrations
{
    public partial class fixcartordership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_DogItem_DogItemId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_DogProductItem_DogProductItemId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShipInfoId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "ShipInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ShipInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "ShipInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ShipInfo",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DogProductItemId",
                table: "OrderDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DogItemId",
                table: "OrderDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "CartDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShipInfo_UserId",
                table: "ShipInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShipInfoId",
                table: "Order",
                column: "ShipInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_DogItem_DogItemId",
                table: "OrderDetail",
                column: "DogItemId",
                principalTable: "DogItem",
                principalColumn: "DogItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_DogProductItem_DogProductItemId",
                table: "OrderDetail",
                column: "DogProductItemId",
                principalTable: "DogProductItem",
                principalColumn: "DogProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipInfo_AspNetUsers_UserId",
                table: "ShipInfo",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_DogItem_DogItemId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_DogProductItem_DogProductItemId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipInfo_AspNetUsers_UserId",
                table: "ShipInfo");

            migrationBuilder.DropIndex(
                name: "IX_ShipInfo_UserId",
                table: "ShipInfo");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShipInfoId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShipInfo");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartDetail");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "ShipInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ShipInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "ShipInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DogProductItemId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DogItemId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShipInfoId",
                table: "Order",
                column: "ShipInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_DogItem_DogItemId",
                table: "OrderDetail",
                column: "DogItemId",
                principalTable: "DogItem",
                principalColumn: "DogItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_DogProductItem_DogProductItemId",
                table: "OrderDetail",
                column: "DogProductItemId",
                principalTable: "DogProductItem",
                principalColumn: "DogProductItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
