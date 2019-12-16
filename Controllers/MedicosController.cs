using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AgendaConsultorio.Models;

namespace AgendaConsultorio.Controllers
{
    public class MedicosController : Controller
    {
        public IActionResult Index()
        {
            List<Medico> list = new List<Medico>();
            list.Add(new Medico { Registro = 1, Nome = "Dr Dolittle"});
            list.Add(new Medico { Registro = 2, Nome = "Dr Frankenstein"});

            return View(list);
        }
    }
}