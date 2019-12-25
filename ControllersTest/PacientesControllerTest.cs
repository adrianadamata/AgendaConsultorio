using AgendaConsultorio.Controllers;
using NUnit.Framework;
using AgendaConsultorio.Services;
using AgendaConsultorio.Models;
using AgendaConsultorio.Migrations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgendaConcultorioTest.ControllersTest
{
    class PacientesControllerTest
    {
        private readonly AgendaConsultorioContext _context;

        private readonly PacienteService _pacienteService;

        private readonly MedicoService _medicoService;

        [Test]
        public async Task TestDetailsView()
        {
            var controller = new PacientesController(_pacienteService, _medicoService, _context);
            var result = await controller.Details() as ViewResult;
            Assert.AreEqual("Details", result.ViewName);

        }

        [Test]
        public async Task PacienteCreateTest()
        {
            var pessoa = new Faker<Paciente>("pt_BR")
                .RuleFor(c => c.Id, f => f.Random.Int())
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.BirthDate, f => f.Date.Past(15))
                .RuleFor(c => c.DateTimeInitial, f => f.Date.Recent())
                .RuleFor(c => c.DateTimeEnd, f => f.Date.Recent())
                .RuleFor(c => c.Comments, " ")
                .RuleFor(c => c.MedicoId, (_medicoService.AnyMedicoAsync()).Id)
                .Generate();
            bool dateInvalid = (DateTime.Compare(pessoa.DateTimeInitial, pessoa.DateTimeEnd) >= 0);
            if (!dateInvalid)
            {
                bool hasAnyHour = await _context.Paciente.AnyAsync(x =>
               DateTime.Compare(x.DateTimeEnd, pessoa.DateTimeInitial) > 0 &&
               DateTime.Compare(x.DateTimeInitial, pessoa.DateTimeEnd) < 0);

                if (!hasAnyHour)
                {
                }
            }
            int expected = await (_pacienteService.InsertAsync(pessoa)).Id;
            Assert.AreEqual(expected, pessoa.Id);
        }

    }
}
