using Microservicio_Administracion.Administracion;
using System.ComponentModel.DataAnnotations;
namespace Microservicio_Administracion.Models

{
    public class ConsultasMedicas
    {
        [Key]
        public required int id_consulta_medica { set; get; }
        public required DateOnly fecha { set; get; }
        public required string hora { set; get; }
        public required string motivo { set; get; }
        public required string diagnostico { set; get; }
        public required string tratamiento { set; get; }

        public required int id_empleado { get; set; }
        public required Paciente paciente { set; get; }
    }
}
