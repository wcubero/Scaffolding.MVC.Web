using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public JsonResult GetListadoTest(Persona persona)
        {
            List<Persona> ListPersonas = new List<Persona>();

            ListPersonas.Add(new Persona(){ Id = 1, Nombre = "Walter", Apellido = "Cubero", Edad = 32  });

            ListPersonas.Add(new Persona() { Id = 2, Nombre = "Alonso", Apellido = "Gamboa", Edad = 24 });

            for (int i = 0; i < 100; i++)
            {
                ListPersonas.Add(new Persona() { Id = i+2, Nombre = "Alonso", Apellido = "Gamboa", Edad = 24 });
            }

            return Json(ListPersonas);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
