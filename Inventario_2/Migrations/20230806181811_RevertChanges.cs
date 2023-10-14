using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario_2.Migrations
{
    /// <inheritdoc />
    public partial class RevertChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Almacenes",
                columns: table => new
                {
                    IdAlmacenes = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Almacene__A56E336995113A9F", x => x.IdAlmacenes);
                });

            migrationBuilder.CreateTable(
                name: "AsientoContable",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Auxiliar = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((4))"),
                    CuentaDB = table.Column<int>(type: "int", nullable: false),
                    CuentaCR = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AsientoC__881A6AE0DD6D7CF1", x => x.IdMovimiento);
                });

            migrationBuilder.CreateTable(
                name: "CuentaContable",
                columns: table => new
                {
                    IdCuentaContable = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CuentaCo__458CB9B223C1455F", x => x.IdCuentaContable);
                });

            migrationBuilder.CreateTable(
                name: "TiposInventario",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CuentaContable = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TiposInv__1927B20C54F04ACA", x => x.IdInventario);
                });

            migrationBuilder.CreateTable(
                name: "Articulos",
                columns: table => new
                {
                    IdArticulos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Existencia = table.Column<int>(type: "int", nullable: false),
                    IdInventario = table.Column<int>(type: "int", nullable: true),
                    CostoUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estado = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Articulo__A1E947759EF05868", x => x.IdArticulos);
                    table.ForeignKey(
                        name: "FK_TiposInventario",
                        column: x => x.IdInventario,
                        principalTable: "TiposInventario",
                        principalColumn: "IdInventario");
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    IdTransaccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoTransaccion = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: false),
                    IdArticulos = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transacc__334B1F77D762842D", x => x.IdTransaccion);
                    table.ForeignKey(
                        name: "FK_Articulo",
                        column: x => x.IdArticulos,
                        principalTable: "Articulos",
                        principalColumn: "IdArticulos");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articulos_IdInventario",
                table: "Articulos",
                column: "IdInventario");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_IdArticulos",
                table: "Transacciones",
                column: "IdArticulos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Almacenes");

            migrationBuilder.DropTable(
                name: "AsientoContable");

            migrationBuilder.DropTable(
                name: "CuentaContable");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Articulos");

            migrationBuilder.DropTable(
                name: "TiposInventario");
        }
    }
}
