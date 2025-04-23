using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservicio_ConsultasMedicas.Migrations
{
    /// <inheritdoc />
    public partial class idcentroMedicoconsultas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_centro_medico",
                table: "ConsultasMedicas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_centro_medico",
                table: "ConsultasMedicas");
        }
    }
}
