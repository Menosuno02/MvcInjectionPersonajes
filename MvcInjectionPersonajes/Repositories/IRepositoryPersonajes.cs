using MvcInjectionPersonajes.Models;

namespace MvcInjectionPersonajes.Repositories
{
    public interface IRepositoryPersonajes
    {
        public List<Personaje> GetPersonajes();
        public void CreatePersonaje(Personaje personaje);
        public Personaje FindPersonaje(int id);
        public void UpdatePersonaje(Personaje personaje);
        public void DeletePersonaje(int id);
        public List<Personaje> FiltrarPersonajes(string nombre);
    }
}
