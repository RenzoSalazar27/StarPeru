using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeroLinea.Migrations
{
    /// <inheritdoc />
    public partial class Hola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pasajeros",
                columns: table => new
                {
                    idPasajero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombresPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellidosPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dniPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefonoPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaNacimientoPasajero = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paisOrigenPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ciudadOrigenPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigoPostalPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordPasajero = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasajeros", x => x.idPasajero);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pasajeros");
        }
    }
}
