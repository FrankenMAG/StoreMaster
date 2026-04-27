using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreMaster.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Actualizacion_CFDI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsoCFDI",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsoCFDI",
                table: "Clientes");
        }
    }
}
