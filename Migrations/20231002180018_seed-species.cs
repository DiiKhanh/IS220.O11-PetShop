using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Migrations
{
    public partial class seedspecies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DogSpecies",
                columns: new[] { "DogSpeciesId", "CreateAt", "DogSpeciesName", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, "Golden Retriever", null, null },
                    { 2, null, "Alaska", null, null },
                    { 3, null, "Husky", null, null },
                    { 4, null, "Corgi", null, null },
                    { 5, null, "Doberman", null, null },
                    { 6, null, "Pitbull", null, null },
                    { 7, null, "Lạp Xưởng", null, null },
                    { 8, null, "Poodle", null, null },
                    { 9, null, "Chihuahua", null, null },
                    { 10, null, "Shiba", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DogSpecies",
                keyColumn: "DogSpeciesId",
                keyValue: 10);
        }
    }
}
