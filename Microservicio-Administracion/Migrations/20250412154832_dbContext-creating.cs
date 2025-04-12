using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservicio_Administracion.Migrations
{
    /// <inheritdoc />
    public partial class dbContextcreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios",
                column: "empleadoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_empleadoId",
                table: "Usuarios",
                column: "empleadoId");
        }
    }
}
