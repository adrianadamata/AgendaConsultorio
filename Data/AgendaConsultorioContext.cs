using Microsoft.EntityFrameworkCore;

namespace AgendaConsultorio.Models
{
    public class AgendaConsultorioContext : DbContext
    {
        public AgendaConsultorioContext(DbContextOptions<AgendaConsultorioContext> options)
            : base(options)
        {
        }

        public DbSet<Medico> Medico { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
    }
}
