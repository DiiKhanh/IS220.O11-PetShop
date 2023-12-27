using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Migrations
{
    public partial class addvoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Voucher_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discount_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount_value = table.Column<int>(type: "int", nullable: false),
                    Start_date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    End_date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Max_usage = table.Column<int>(type: "int", nullable: false),
                    Current_usage = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Voucher_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_Code",
                table: "Voucher",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voucher");
        }
    }
}
