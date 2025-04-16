using System.ComponentModel.DataAnnotations;

namespace Microservicio_ConsultasMedicas.Models
{
    public class Paciente
    {
        [Key]
        public int id_paciente { get; set; }

        public required string nombre { get; set; }


        public required string cedula { get; set; }


        public required DateOnly fecha_nacimiento { get; set; }


        public required string telefono { get; set; }


        public required string direccion { get; set; }

    }
}
