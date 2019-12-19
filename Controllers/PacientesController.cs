using System;
using System.Collections.Generic;
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
            _pacienteService.Insert(paciente);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _pacienteService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
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
                return NotFound();
            }
            var obj = _pacienteService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _pacienteService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            List<Medico> medicos = _medicoService.FindAll();
            PacienteFormViewModel viewModel = new PacienteFormViewModel { Paciente = obj, Medicos = medicos };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return BadRequest();
            }
            try
            {
                _pacienteService.Update(paciente);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}