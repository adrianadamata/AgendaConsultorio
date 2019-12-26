using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaConsultorio.Models
{
    public class Paciente
    {
        public bool IsValid { get; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome do Paciente")]
        public string Name { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Início da Consulta")]
        public DateTime DateTimeInitial { get; set; }

        [Display(Name = "Fim da Consulta")]
        public DateTime DateTimeEnd { get; set; }

        [Display(Name = "Observações")]
        public string Comments { get; set; }

        [Display(Name = "Médico")]
        public Medico Medico { get; set; }

        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

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
    }
}
