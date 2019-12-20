using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AgendaConsultorio.Models;
using AgendaConsultorio.Models.ViewModels;
using AgendaConsultorio.Services;
using AgendaConsultorio.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgendaConsultorio.Controllers
{
    public class PacientesController : Controller
    {
        private readonly PacienteService _pacienteService;

        private readonly MedicoService _medicoService;

        public PacientesController(PacienteService pacienteService, MedicoService medicoService)
        {
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        public IActionResult Index()
        {
            var list = _pacienteService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var medicos = _medicoService.FindAll();
            var viewModel = new PacienteFormViewModel { Medicos = medicos };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paciente paciente)
        {
            if (!ModelState.IsValid)
            {
                var medicos = _medicoService.FindAll();
                var viewModel = new PacienteFormViewModel { Paciente = paciente, Medicos = medicos };
                return View(viewModel);
            }
            _pacienteService.Insert(paciente);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = _pacienteService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _pacienteService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = _pacienteService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = _pacienteService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Medico> medicos = _medicoService.FindAll();
            PacienteFormViewModel viewModel = new PacienteFormViewModel { Paciente = obj, Medicos = medicos };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Paciente paciente)
        {
            if (!ModelState.IsValid)
            {
                var medicos = _medicoService.FindAll();
                var viewModel = new PacienteFormViewModel { Paciente = paciente, Medicos = medicos };
                return View(viewModel);
            }
            if (id != paciente.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatched" });
            }
            try
            {
                _pacienteService.Update(paciente);
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