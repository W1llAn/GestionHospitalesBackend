using Microservicio_Administracion.Administracion;
using System.ComponentModel.DataAnnotations;

namespace Microservicio_Administracion.Models
{
    public class ConsultasMedicasEntity
    {
        [Key]
        public int id_consulta_medica { set; get; }
        public  DateOnly fecha { set; get; }
        public  string hora { set; get; }
        public  string motivo { set; get; }
        public  string diagnostico { set; get; }
        public  string tratamiento { set; get; }
        public  int id_empleado { get; set; }
        public  Paciente paciente { set; get; }

        // Constructor personalizado
        public ConsultasMedicasEntity(DateOnly fecha, string hora, string motivo, string diagnostico, string tratamiento, int id_empleado, Paciente paciente)
        {
            this.fecha = fecha;
            this.hora = hora;
            this.motivo = motivo;
            this.diagnostico = diagnostico;
            this.tratamiento = tratamiento;
            this.id_empleado = id_empleado;
            this.paciente = paciente;
        }

        // Constructor sin parámetros (requerido por Entity Framework)
        public ConsultasMedicasEntity()
        {
        }
    }
}