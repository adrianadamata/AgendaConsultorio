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

        public List<Paciente> FindAll()
        {
            return _context.Paciente.OrderBy(x => x.DateTimeInitial).ToList();
        }

        public void Insert(Paciente obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Paciente FindById(int id)
        {
            return _context.Paciente.Include(obj => obj.Medico).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Paciente.Find(id);
            _context.Paciente.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Paciente obj)
        {
            if (!_context.Paciente.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
