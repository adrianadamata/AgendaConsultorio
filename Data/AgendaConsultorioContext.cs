using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgendaConsultorio.Models
{
    public class AgendaConsultorioContext : DbContext
    {
        public AgendaConsultorioContext (DbContextOptions<AgendaConsultorioContext> options)
            : base(options)
        {
        }

        public DbSet<AgendaConsultorio.Models.Medico> Medico { get; set; }
    }
}
