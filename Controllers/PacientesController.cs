using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AgendaConsultorio.Models;
using AgendaConsultorio.Models.ViewModels;
using AgendaConsultorio.Services;
using AgendaConsultorio.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AgendaConsultorio.Controllers
{
    public class PacientesController : Controller
    {
        private readonly AgendaConsultorioContext _context;

        private readonly PacienteService _pacienteService;

        private readonly MedicoService _medicoService;

        public PacientesController(PacienteService pacienteService, MedicoService medicoService, AgendaConsultorioContext context)
        {
            _context = context;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        public async Task<IActionResult> Index(string search)
        {
            ViewData["search"] = search;

            var pacientes = from pac in _context.Paciente select pac;
            if (!String.IsNullOrEmpty(search))
            {
                pacientes = pacientes.Where(s => s.Name.Contains(search));
            }
            return View(await pacientes.AsNoTracking().ToListAsync());
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
            bool hasAnyInitial = await _context.Paciente.AnyAsync(x =>
            DateTime.Compare(x.DateTimeInitial, paciente.DateTimeInitial) <= 0 &&
            DateTime.Compare(x.DateTimeEnd, paciente.DateTimeInitial) > 0);

            bool hasAnyEnd = await _context.Paciente.AnyAsync(x =>
            DateTime.Compare(x.DateTimeInitial, paciente.DateTimeEnd) <= 0 &&
            DateTime.Compare(x.DateTimeEnd, paciente.DateTimeEnd) > 0);

            if (hasAnyInitial)
            {
                ModelState.AddModelError("DateTimeInitial", "Este horário já está agendado.");
            }
            else if (hasAnyEnd)
            {
                ModelState.AddModelError("DateTimeEnd", "Este horário já está agendado.");
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
    }
}