using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreMaster.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Agregar_Campo_cliente_Factura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreReceptor",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RFCReceptor",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegimenFiscalReceptor",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreReceptor",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "RFCReceptor",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "RegimenFiscalReceptor",
                table: "Facturas");
        }
    }
}
