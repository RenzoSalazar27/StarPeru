using Microsoft.EntityFrameworkCore.Migrations;

namespace AeroLinea.Data.Migrations
{
    public partial class AddRoutePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "route_prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:Identity", "1, 1"),
                    OriginId = table.Column<int>(type: "int", nullable: false),
                    DestinyId = table.Column<int>(type: "int", nullable: false),
                    EconomyPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ExecutivePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PremiumPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_route_prices_locations_OriginId",
                        column: x => x.OriginId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_route_prices_locations_DestinyId",
                        column: x => x.DestinyId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_route_prices_OriginId",
                table: "route_prices",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_route_prices_DestinyId",
                table: "route_prices",
                column: "DestinyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "route_prices");
        }
    }
} 