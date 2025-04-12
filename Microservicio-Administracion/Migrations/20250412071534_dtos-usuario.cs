using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservicio_Administracion.Migrations
{
    /// <inheritdoc />
    public partial class dtosusuario : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios",
                column: "empleadoId");

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
                name: "FK_Usuarios_Empleados_empleadoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "empleadoId",
                table: "Usuarios");
        }
    }
}
