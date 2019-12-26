using AgendaConsultorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgendaConsultorio.Services
{
    public class MedicoService
    {
        private readonly AgendaConsultorioContext _context;

        public MedicoService(AgendaConsultorioContext context)
        {
            _context = context;
        }
        public async Task<List<Medico>> FindAllAsync()
        {
            return await _context.Medico.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<bool> AnyMedicoAsync()
        {
            return await _context.Medico.AnyAsync();
        }
    }
}
