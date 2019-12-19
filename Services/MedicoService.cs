using AgendaConsultorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaConsultorio.Services
{
    public class MedicoService
    {
        private readonly AgendaConsultorioContext _context;

        public MedicoService(AgendaConsultorioContext context)
        {
            _context = context;
        }
        public List<Medico> FindAll()
        {
            return _context.Medico.OrderBy(x => x.Name).ToList();
        }
    }
}
