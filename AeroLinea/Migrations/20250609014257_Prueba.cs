using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeroLinea.Migrations
{
    /// <inheritdoc />
    public partial class Prueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombresUsuario = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellidosUsuario = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nacimientoUsuario = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    telefonoUsuario = table.Column<int>(type: "int", nullable: false),
                    identificacionUsuario = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    correoUsuario = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contrasenaUsuario = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    discapacidad = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    esAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idUsuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
