using AgendaConsultorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AgendaConsultorio.Services.Exceptions;

namespace AgendaConsultorio.Services
{
    public class PacienteService
    {
        private readonly AgendaConsultorioContext _context;

        public PacienteService(AgendaConsultorioContext context)
        {
            _context = context;
        }

        public async Task<List<Paciente>> FindAllAsync()
        {
            return await _context.Paciente.OrderBy(x => x.DateTimeInitial).ToListAsync();

        }

        public async Task InsertAsync(Paciente obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Paciente> FindByIdAsync(int id)
        {
            return await _context.Paciente.Include(obj => obj.Medico).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Paciente.FindAsync(id);
            _context.Paciente.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Paciente obj)
        {

            bool hasAny = await _context.Paciente.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
