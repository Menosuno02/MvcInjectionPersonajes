using Microsoft.AspNetCore.Mvc;
using MvcInjectionPersonajes.Models;
using MvcInjectionPersonajes.Repositories;

namespace MvcInjectionPersonajes.Controllers
{
    public class PersonajesController : Controller
    {
        private IRepositoryPersonajes repo;

        public PersonajesController(IRepositoryPersonajes repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Personaje> personajes = this.repo.GetPersonajes();
            return View(personajes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Personaje personaje)
        {
            this.repo.CreatePersonaje(personaje);
            return RedirectToAction("Index");
        }
    }
}
