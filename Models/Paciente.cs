
namespace AgendaConsultorio.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateTimeInitial { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string Comments { get; set; }
        public Medico Medico { get; set; }

        public Paciente()
        {

        }

        public Paciente(int id, string name, DateTime birthDate, DateTime dateTimeInitial, DateTime dateTimeEnd, string comments, Medico medico)
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            DateTimeInitial = dateTimeInitial;
            DateTimeEnd = dateTimeEnd;
            Comments = comments;
            Medico = medico;
        }
        public void AddPaciente()
        {

        }
    }
}
