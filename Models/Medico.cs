using System.Collections.Generic;

namespace AgendaConsultorio.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
        public Medico()
        {

        }

        public Medico(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
