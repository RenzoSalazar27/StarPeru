using Microsoft.EntityFrameworkCore.Migrations;

namespace AeroLinea.Data.Migrations
{
    public partial class InsertRoutePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertar los precios de las rutas
            migrationBuilder.InsertData(
                table: "route_prices",
                columns: new[] { "OriginId", "DestinyId", "EconomyPrice", "ExecutivePrice", "PremiumPrice" },
                values: new object[,]
                {
                    { 1, 2, 300.00m, 450.00m, 700.00m },  // Lima → Iquitos
                    { 1, 3, 150.00m, 250.00m, 400.00m },  // Lima → Huánuco
                    { 4, 3, 120.00m, 200.00m, 350.00m },  // Tarapoto → Huánuco
                    { 2, 4, 130.00m, 210.00m, 360.00m },  // Iquitos → Tarapoto
                    { 4, 5, 140.00m, 220.00m, 370.00m },  // Tarapoto → Pucallpa
                    { 5, 6, 180.00m, 280.00m, 450.00m },  // Pucallpa → Cajamarca
                    { 6, 7, 100.00m, 180.00m, 300.00m }   // Cajamarca → Chiclayo
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar los datos insertados
            migrationBuilder.DeleteData(
                table: "route_prices",
                keyColumn: "OriginId",
                keyValues: new object[] { 1, 1, 4, 2, 4, 5, 6 }
            );
        }
    }
} 