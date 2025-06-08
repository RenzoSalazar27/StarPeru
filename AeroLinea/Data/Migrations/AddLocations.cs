using Microsoft.EntityFrameworkCore.Migrations;

namespace AeroLinea.Data.Migrations
{
    public partial class AddLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertar las ubicaciones necesarias
            migrationBuilder.InsertData(
                table: "locations",
                columns: new[] { "Id", "Name", "Code", "IsActive" },
                values: new object[,]
                {
                    { 1, "Lima", "LIM", true },
                    { 2, "Iquitos", "IQT", true },
                    { 3, "Huánuco", "HUU", true },
                    { 4, "Tarapoto", "TPP", true },
                    { 5, "Pucallpa", "PCL", true },
                    { 6, "Cajamarca", "CJA", true },
                    { 7, "Chiclayo", "CIX", true }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar las ubicaciones insertadas
            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7 }
            );
        }
    }
} 