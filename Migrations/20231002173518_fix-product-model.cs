using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Migrations
{
    public partial class fixproductmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DogProductItem_DogProductType_DogProductTypeId",
                table: "DogProductItem");

            migrationBuilder.DropTable(
                name: "DogProductType");

            migrationBuilder.DropIndex(
                name: "IX_DogProductItem_DogProductTypeId",
                table: "DogProductItem");

            migrationBuilder.DropColumn(
                name: "DogProductTypeId",
                table: "DogProductItem");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "DogProductItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "DogProductItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Images",
                table: "DogItem",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "DogProductItem");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DogProductItem");

            migrationBuilder.AddColumn<int>(
                name: "DogProductTypeId",
                table: "DogProductItem",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Images",
                table: "DogItem",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.CreateTable(
                name: "DogProductType",
                columns: table => new
                {
                    DogProductTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogProductType", x => x.DogProductTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DogProductItem_DogProductTypeId",
                table: "DogProductItem",
                column: "DogProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DogProductItem_DogProductType_DogProductTypeId",
                table: "DogProductItem",
                column: "DogProductTypeId",
                principalTable: "DogProductType",
                principalColumn: "DogProductTypeId");
        }
    }
}
