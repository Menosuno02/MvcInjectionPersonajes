using MvcInjectionPersonajes.Models;

namespace MvcInjectionPersonajes.Repositories
{
    public interface IRepositoryPersonajes
    {
        public List<Personaje> GetPersonajes();
        public void CreatePersonaje(Personaje personaje);
    }
}
