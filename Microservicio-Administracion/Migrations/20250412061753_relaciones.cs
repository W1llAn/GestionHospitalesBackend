using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservicio_Administracion.Migrations
{
    /// <inheritdoc />
    public partial class relaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "empleadoId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Centro_MedicoId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EspecialidadId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tipo_EmpleadoId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios",
                column: "empleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Centro_MedicoId",
                table: "Empleados",
                column: "Centro_MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_EspecialidadId",
                table: "Empleados",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Tipo_EmpleadoId",
                table: "Empleados",
                column: "Tipo_EmpleadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Centros_Medicos_Centro_MedicoId",
                table: "Empleados",
                column: "Centro_MedicoId",
                principalTable: "Centros_Medicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Especialidades_EspecialidadId",
                table: "Empleados",
                column: "EspecialidadId",
                principalTable: "Especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Tipos_Empleados_Tipo_EmpleadoId",
                table: "Empleados",
                column: "Tipo_EmpleadoId",
                principalTable: "Tipos_Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Empleados_empleadoId",
                table: "Usuarios",
                column: "empleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Centros_Medicos_Centro_MedicoId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Especialidades_EspecialidadId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Tipos_Empleados_Tipo_EmpleadoId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Empleados_empleadoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_Centro_MedicoId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_EspecialidadId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_Tipo_EmpleadoId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "empleadoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Centro_MedicoId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "EspecialidadId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "Tipo_EmpleadoId",
                table: "Empleados");
        }
    }
}
