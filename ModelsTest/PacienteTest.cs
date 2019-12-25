using NUnit.Framework;
using AgendaConsultorio.Models;
using AgendaConsultorio.Models.ViewModels;
using Bogus;
using AgendaConsultorio.Services;
using System.Threading.Tasks;

namespace AgendaConcultorioTest.ModelsTest
{
    class PacienteTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        private readonly MedicoService _medicoService;
        public PacienteTest(MedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [Test]
        public async Task PacienteNameTest()
        {
            Paciente paciente = new Faker<Paciente>("pt_BR")
               .RuleFor(c => c.Id, f => f.Random.Int())
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.BirthDate, f => f.Date.Past(15))
                .RuleFor(c => c.DateTimeInitial, f => f.Date.Recent())
                .RuleFor(c => c.DateTimeEnd, f => f.Date.Recent())
                .RuleFor(c => c.Comments, " ")
                .RuleFor(c => c.MedicoId, (_medicoService.AnyMedicoAsync()).Id)
                .Generate();

            bool isValid = paciente.IsValid;

            Assert.IsTrue(isValid);
        }
    }
}
