using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeroLinea.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutePrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "route_prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OriginId = table.Column<int>(type: "int", nullable: false),
                    DestinyId = table.Column<int>(type: "int", nullable: false),
                    EconomyPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ExecutivePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PremiumPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_route_prices_locations_DestinyId",
                        column: x => x.DestinyId,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_route_prices_locations_OriginId",
                        column: x => x.OriginId,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_route_prices_DestinyId",
                table: "route_prices",
                column: "DestinyId");

            migrationBuilder.CreateIndex(
                name: "IX_route_prices_OriginId",
                table: "route_prices",
                column: "OriginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "route_prices");
        }
    }
}
