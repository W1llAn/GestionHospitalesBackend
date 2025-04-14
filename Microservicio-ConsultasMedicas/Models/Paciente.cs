namespace Microservicio_ConsultasMedicas.Models
{
    public class Paciente
    {
        public int id_paciente { get; set; }

        public string nombre { get; set; }

        public string cedula { get; set; }

        public DateOnly fecha_nacimiento { get; set; }

        public string telefono { get; set; }

        public string direccion { get; set; }
    }
}
