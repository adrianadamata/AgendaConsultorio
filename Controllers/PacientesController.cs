using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgendaConsultorio.Models;
using AgendaConsultorio.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendaConsultorio.Controllers
{
    public class PacientesController : Controller
    {
        private readonly PacienteService _pacienteService;
        public PacientesController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        public IActionResult Index()
        {
            var list = _pacienteService.FindAll();
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (Paciente paciente)
        {
            _pacienteService.Insert(paciente);
            return RedirectToAction(nameof(Index));
        }
    }
}