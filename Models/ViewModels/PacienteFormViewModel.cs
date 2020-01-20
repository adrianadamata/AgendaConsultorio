using System.Collections.Generic;

namespace AgendaConsultorio.Models.ViewModels
{
    public class PacienteFormViewModel
    {
        public Paciente Paciente { get; set; }

        public ICollection<Medico> Medicos { get; set; }
    }
}
