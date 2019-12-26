using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
