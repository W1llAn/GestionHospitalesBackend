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

        public DbSet<Paciente> Paciente { get; set; }
    }
}
