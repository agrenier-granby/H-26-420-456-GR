using Exercices.Fetch.DTO;
using Exercices.Fetch.Models;
using Exercices.Fetch.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exercices.Fetch.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public IActionResult Partie1()
        {
            return NoContent();
        }

        [HttpPost]
        public IActionResult Partie2()
        {
            return NotFound();
        }

        [HttpPost]
        public IActionResult Partie3([FromBody] MesInfosDTO dto)
        {
            dto.Age = 50;

            return Json(dto);
        }

        [HttpPost]
        public IActionResult Partie4([FromBody] MesInfosDTO dto)
        {
            dto.Age = 50;

            MesInfosVM infos = new()
            {
                Nom = dto.Nom,
                Age = dto.Age,
                VilleResidence = dto.VilleResidence
            };

            return PartialView("_MesInfosPartial", infos);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Partie5([FromBody] MesInfosDTO dto)
        {
            dto.Age = 40;

            MesInfosVM infos = new()
            {
                Nom = dto.Nom,
                Age = dto.Age,
                VilleResidence = dto.VilleResidence
            };

            return PartialView("_MesInfosPartial", infos);
        }
        public IActionResult ObtenirVueInfosAleatoire()
        {
            return PartialView("_ObtenirInfosAleatoirePartial");
        }

        [HttpPost]
        public IActionResult ObtenirInfosAleatoire()
        {
            var dto = new MesInfosVM
            {
                Nom = "Toto",
                Age = Random.Shared.Next(18, 65),
                VilleResidence = "Paris"
            };

            return PartialView("_MesInfosPartial", dto);

        }
    }
}