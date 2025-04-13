using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservicio_Administracion.Migrations
{
    /// <inheritdoc />
    public partial class dtosEmpleados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_tipo",
                table: "Empleados",
                newName: "tipo_empleadoID");

            migrationBuilder.RenameColumn(
                name: "id_especialidad",
                table: "Empleados",
                newName: "especialidadID");

            migrationBuilder.RenameColumn(
                name: "id_centro_medico",
                table: "Empleados",
                newName: "centro_medicoID");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_centro_medicoID",
                table: "Empleados",
                column: "centro_medicoID");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_especialidadID",
                table: "Empleados",
                column: "especialidadID");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_tipo_empleadoID",
                table: "Empleados",
                column: "tipo_empleadoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Centros_Medicos_centro_medicoID",
                table: "Empleados",
                column: "centro_medicoID",
                principalTable: "Centros_Medicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Especialidades_especialidadID",
                table: "Empleados",
                column: "especialidadID",
                principalTable: "Especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Tipos_Empleados_tipo_empleadoID",
                table: "Empleados",
                column: "tipo_empleadoID",
                principalTable: "Tipos_Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Centros_Medicos_centro_medicoID",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Especialidades_especialidadID",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Tipos_Empleados_tipo_empleadoID",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_centro_medicoID",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_especialidadID",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_tipo_empleadoID",
                table: "Empleados");

            migrationBuilder.RenameColumn(
                name: "tipo_empleadoID",
                table: "Empleados",
                newName: "id_tipo");

            migrationBuilder.RenameColumn(
                name: "especialidadID",
                table: "Empleados",
                newName: "id_especialidad");

            migrationBuilder.RenameColumn(
                name: "centro_medicoID",
                table: "Empleados",
                newName: "id_centro_medico");
        }
    }
}
