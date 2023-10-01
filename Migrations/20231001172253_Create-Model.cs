using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Migrations
{
    public partial class CreateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                });

            migrationBuilder.CreateTable(
                name: "DogProductType",
                columns: table => new
                {
                    DogProductTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogProductType", x => x.DogProductTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DogSpecies",
                columns: table => new
                {
                    DogSpeciesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DogSpeciesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogSpecies", x => x.DogSpeciesId);
                });

            migrationBuilder.CreateTable(
                name: "ShipInfo",
                columns: table => new
                {
                    ShipInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipInfo", x => x.ShipInfoId);
                });

            migrationBuilder.CreateTable(
                name: "CartDetail",
                columns: table => new
                {
                    CartDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductItemId = table.Column<int>(type: "int", nullable: true),
                    DogItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetail", x => x.CartDetailId);
                    table.ForeignKey(
                        name: "FK_CartDetail_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "CartId");
                });

            migrationBuilder.CreateTable(
                name: "DogProductItem",
                columns: table => new
                {
                    DogProductItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DogProductTypeId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsInStock = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogProductItem", x => x.DogProductItemId);
                    table.ForeignKey(
                        name: "FK_DogProductItem_DogProductType_DogProductTypeId",
                        column: x => x.DogProductTypeId,
                        principalTable: "DogProductType",
                        principalColumn: "DogProductTypeId");
                });

            migrationBuilder.CreateTable(
                name: "DogItem",
                columns: table => new
                {
                    DogItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DogName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsInStock = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DogSpeciesId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogItem", x => x.DogItemId);
                    table.ForeignKey(
                        name: "FK_DogItem_DogSpecies_DogSpeciesId",
                        column: x => x.DogSpeciesId,
                        principalTable: "DogSpecies",
                        principalColumn: "DogSpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
                    ShipInfoId = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: true),
                    ShipmentStatus = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_ShipInfo_ShipInfoId",
                        column: x => x.ShipInfoId,
                        principalTable: "ShipInfo",
                        principalColumn: "ShipInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartDetailDogProductItem",
                columns: table => new
                {
                    cartDetailsCartDetailId = table.Column<int>(type: "int", nullable: false),
                    dogProductItemsDogProductItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetailDogProductItem", x => new { x.cartDetailsCartDetailId, x.dogProductItemsDogProductItemId });
                    table.ForeignKey(
                        name: "FK_CartDetailDogProductItem_CartDetail_cartDetailsCartDetailId",
                        column: x => x.cartDetailsCartDetailId,
                        principalTable: "CartDetail",
                        principalColumn: "CartDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetailDogProductItem_DogProductItem_dogProductItemsDogProductItemId",
                        column: x => x.dogProductItemsDogProductItemId,
                        principalTable: "DogProductItem",
                        principalColumn: "DogProductItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartDetailDogItem",
                columns: table => new
                {
                    cartDetailsCartDetailId = table.Column<int>(type: "int", nullable: false),
                    dogItemsDogItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetailDogItem", x => new { x.cartDetailsCartDetailId, x.dogItemsDogItemId });
                    table.ForeignKey(
                        name: "FK_CartDetailDogItem_CartDetail_cartDetailsCartDetailId",
                        column: x => x.cartDetailsCartDetailId,
                        principalTable: "CartDetail",
                        principalColumn: "CartDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetailDogItem_DogItem_dogItemsDogItemId",
                        column: x => x.dogItemsDogItemId,
                        principalTable: "DogItem",
                        principalColumn: "DogItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoice_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DogItemId = table.Column<int>(type: "int", nullable: false),
                    DogProductItemId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_DogItem_DogItemId",
                        column: x => x.DogItemId,
                        principalTable: "DogItem",
                        principalColumn: "DogItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_DogProductItem_DogProductItemId",
                        column: x => x.DogProductItemId,
                        principalTable: "DogProductItem",
                        principalColumn: "DogProductItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_CartId",
                table: "CartDetail",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetailDogItem_dogItemsDogItemId",
                table: "CartDetailDogItem",
                column: "dogItemsDogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetailDogProductItem_dogProductItemsDogProductItemId",
                table: "CartDetailDogProductItem",
                column: "dogProductItemsDogProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DogItem_DogSpeciesId",
                table: "DogItem",
                column: "DogSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_DogProductItem_DogProductTypeId",
                table: "DogProductItem",
                column: "DogProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_OrderId",
                table: "Invoice",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShipInfoId",
                table: "Order",
                column: "ShipInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_DogItemId",
                table: "OrderDetail",
                column: "DogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_DogProductItemId",
                table: "OrderDetail",
                column: "DogProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDetailDogItem");

            migrationBuilder.DropTable(
                name: "CartDetailDogProductItem");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "CartDetail");

            migrationBuilder.DropTable(
                name: "DogItem");

            migrationBuilder.DropTable(
                name: "DogProductItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "DogSpecies");

            migrationBuilder.DropTable(
                name: "DogProductType");

            migrationBuilder.DropTable(
                name: "ShipInfo");
        }
    }
}
