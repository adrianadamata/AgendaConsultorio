using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AgendaConsultorio.Models;
using AgendaConsultorio.Models.ViewModels;
using AgendaConsultorio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaConsultorio.Controllers
{
    public class PacientesController : Controller
    {
        private readonly AgendaConsultorioContext _context;

        private readonly PacienteService _pacienteService;

        private readonly MedicoService _medicoService;

        public async Task<IActionResult> IndexPaciente(string search)
        {
            ViewData["search"] = search;

            var pacientes = from pac in _context.Paciente select pac;
            if (!String.IsNullOrEmpty(search))
            {
                pacientes = pacientes.Where(s => s.Name.Contains(search));
            }
            return View(await pacientes.OrderBy(s => s.DateTimeInitial).AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Index(string date)
        {
            ViewData["date"] = date;

            var pacientes = from pac in _context.Paciente select pac;
            if (!String.IsNullOrEmpty(date))
            {
                pacientes = pacientes.Where(s => (s.DateTimeInitial.Date.ToString() == date));
            }

            return View(await pacientes.OrderBy(s => s.DateTimeInitial).AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var medicos = await _medicoService.FindAllAsync();
            var viewModel = new PacienteFormViewModel { Medicos = medicos };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            bool dateInvalid = (DateTime.Compare(paciente.DateTimeInitial, paciente.DateTimeEnd) >= 0);
            if (dateInvalid)
            {
                ModelState.AddModelError("paciente.DateTimeEnd", "Data final maior");
            }
            else
            {
                bool hasAnyHour = await _context.Paciente.AnyAsync(x =>
                DateTime.Compare(x.DateTimeEnd, paciente.DateTimeInitial) > 0 &&
                DateTime.Compare(x.DateTimeInitial, paciente.DateTimeEnd) < 0);

                if (hasAnyHour)
                {
                    ModelState.AddModelError("paciente.DateTimeInitial", "Horário já agendado.");
                    ModelState.AddModelError("paciente.DateTimeEnd", "Horário já agendado.");
                }
            }

            if (!ModelState.IsValid)
            {
                var medicos = await _medicoService.FindAllAsync();
                var viewModel = new PacienteFormViewModel { Paciente = paciente, Medicos = medicos };
                return View(viewModel);
            }
            await _pacienteService.InsertAsync(paciente);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _pacienteService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _pacienteService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _pacienteService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _pacienteService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Medico> medicos = await _medicoService.FindAllAsync();
            PacienteFormViewModel viewModel = new PacienteFormViewModel { Paciente = obj, Medicos = medicos };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paciente paciente)
        {
            bool dateInvalid = (DateTime.Compare(paciente.DateTimeInitial, paciente.DateTimeEnd) >= 0);
            if (dateInvalid)
            {
                ModelState.AddModelError("paciente.DateTimeEnd", "Data final menor");
            }
            else
            {
                bool hasAnyHour = await _context.Paciente.AnyAsync(x =>
                DateTime.Compare(x.DateTimeEnd, paciente.DateTimeInitial) > 0 &&
                DateTime.Compare(x.DateTimeInitial, paciente.DateTimeEnd) < 0 &&
                (x.Id != paciente.Id));

                if (hasAnyHour)
                {
                    ModelState.AddModelError("paciente.DateTimeInitial", "Horário já agendado.");
                    ModelState.AddModelError("paciente.DateTimeEnd", "Horário já agendado.");
                }
            }

            if (!ModelState.IsValid)
            {
                var medicos = await _medicoService.FindAllAsync();
                var viewModel = new PacienteFormViewModel { Paciente = paciente, Medicos = medicos };
                return View(viewModel);
            }
            if (id != paciente.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatched" });
            }
            try
            {
                await _pacienteService.UpdateAsync(paciente);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        public PacientesController(PacienteService pacienteService, MedicoService medicoService, AgendaConsultorioContext context)
        {
            _context = context;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }
    }
}