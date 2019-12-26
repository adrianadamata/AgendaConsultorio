using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaConsultorio.Models.ViewModels
{
    public class PacienteFormViewModel
    {
        public Paciente Paciente { get; set; }

        public ICollection<Medico> Medicos { get; set; }
    }
}
