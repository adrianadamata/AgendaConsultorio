using AgendaConsultorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaConsultorio.Services
{
    public class PacienteService
    {
        private readonly AgendaConsultorioContext _context;

        public PacienteService (AgendaConsultorioContext context)
        {
            _context = context;
        }

        public List<Paciente> FindAll()
        {
            return _context.Paciente.ToList();
        }

        public void Insert (Paciente obj)
        {
            obj.Medico = _context.Medico.First();
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
