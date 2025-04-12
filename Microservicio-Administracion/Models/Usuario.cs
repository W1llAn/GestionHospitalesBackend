namespace Microservicio_Administracion.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string nombre_usuario { get; set; }
        public string contraseña { get; set; }
        public string id_empleado { get; set; }
        public Empleado empleado { get; set; }
    }
}
