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

        public IActionResult Update(int id)
        {
            Personaje personaje = this.repo.FindPersonaje(id);
            return View(personaje);
        }

        [HttpPost]
        public IActionResult Update(Personaje personaje)
        {
            this.repo.UpdatePersonaje(personaje);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            this.repo.DeletePersonaje(id);
            return RedirectToAction("Index");
        }

        public IActionResult BuscarPersonajes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuscarPersonajes(string nombre)
        {
            List<Personaje> personajes = this.repo.FiltrarPersonajes(nombre);
            if (personajes == null)
            {
                ViewData["MENSAJE"] = "No se encontró personaje con nombre " + nombre;
            }
            return View(personajes);
        }
    }
}
