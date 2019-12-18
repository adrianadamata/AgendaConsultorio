using System.Collections.Generic;
using System.Linq;

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
        public void AddPaciente(Paciente paciente)
        {
            Pacientes.Add(paciente);
        }
        public void RemovePaciente(Paciente paciente)
        {
            Pacientes.Remove(paciente);
        }
//        public Paciente SearchPaciente(string busca)
//        {
//            public List<Paciente> Search = Pacientes.FindAll(x => x.Name == busca);
            
//        }
    }
}
