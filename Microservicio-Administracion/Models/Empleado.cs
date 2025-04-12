namespace Microservicio_Administracion.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public int id_centro_medico { get; set; }
        public int id_tipo { get; set; }
        public string nombre { get; set; }
        public string cedula { get; set; }
        public int id_especialidad { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public double salario { get; set; }

    }
}
