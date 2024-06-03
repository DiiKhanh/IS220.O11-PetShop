using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Migrations
{
    public partial class typespecial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "DogItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "DogSpecies",
                columns: new[] { "DogSpeciesId", "CreateAt", "DogSpeciesName", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { 11, null, "Siamese", null, null },
                    { 12, null, "Maine Coon", null, null },
                    { 13, null, "Persian", null, null },
                    { 14, null, "Bengal", null, null },
                    { 15, null, "Sphynx", null, null },
                    { 16, null, "Munchkin", null, null },
                    { 17, null, "Scottish Fold", null, null },
                    { 18, null, "Đồ cho chó", null, null },
                    { 19, null, "Đồ cho mèo", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 19);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "DogItem");
        }
    }
}
