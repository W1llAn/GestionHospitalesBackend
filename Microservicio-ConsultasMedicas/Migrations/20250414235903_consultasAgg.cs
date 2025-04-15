using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservicio_Administracion.Migrations
{
    /// <inheritdoc />
    public partial class consultasAgg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "id_empleado",
                table: "ConsultasMedicas");

            _ = migrationBuilder.DropColumn(
                name: "id_paciente",
                table: "ConsultasMedicas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "id_empleado",
                table: "ConsultasMedicas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "id_paciente",
                table: "ConsultasMedicas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
