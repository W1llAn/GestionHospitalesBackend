using Microservicio_ConsultasMedicas.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservicio_ConsultasMedicas.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<Microservicio_ConsultasMedicas.Models.ConsultasMedicasEntity> ConsultasMedicas{ get; set; }
        public DbSet<Paciente> Paciente { get; set; }
    }
}
