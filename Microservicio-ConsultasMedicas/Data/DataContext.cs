using Microservicio_Administracion.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservicio_Administracion.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<Microservicio_Administracion.Models.ConsultasMedicas> ConsultasMedicas{ get; set; }
        public DbSet<Paciente> Paciente { get; set; }
    }
}
