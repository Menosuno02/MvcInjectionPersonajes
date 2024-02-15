using MvcInjectionPersonajes.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MvcInjectionPersonajes.Repositories
{
    public class RepositoryPersonajesOracle : IRepositoryPersonajes
    {
        private DataTable tablaPersonajes;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryPersonajesOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            this.tablaPersonajes = new DataTable();
            string sql = "SELECT * FROM PERSONAJES";
            OracleDataAdapter adapter = new OracleDataAdapter(sql, this.cn);
            adapter.Fill(tablaPersonajes);
        }

        public List<Personaje> GetPersonajes()
        {
            var consulta = from datos in this.tablaPersonajes.AsEnumerable()
                           select datos;
            List<Personaje> personajes = new List<Personaje>();
            foreach (var row in consulta)
            {
                Personaje personaje = new Personaje
                {
                    IdPersonaje = row.Field<int>("IDPERSONAJE"),
                    NomPersonaje = row.Field<string>("PERSONAJE"),
                    Imagen = row.Field<string>("IMAGEN")
                };
                personajes.Add(personaje);
            }
            return personajes;
        }

        public void CreatePersonaje(Personaje personaje)
        {
            string sql = "INSERT INTO PERSONAJES VALUES" +
                "(:IDPERSONAJE, :NOMPERSONAJE, :IMAGEN)";
            OracleParameter paramIdPersonaje = new OracleParameter(":IDPERSONAJE", personaje.IdPersonaje);
            this.com.Parameters.Add(paramIdPersonaje);
            OracleParameter paramNomPersonaje = new OracleParameter(":NOMPERSONAJE", personaje.NomPersonaje);
            this.com.Parameters.Add(paramNomPersonaje);
            OracleParameter paramImagen = new OracleParameter(":IMAGEN", personaje.Imagen);
            this.com.Parameters.Add(paramImagen);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Personaje FindPersonaje(int id)
        {
            var consulta = from datos in this.tablaPersonajes.AsEnumerable()
                           where datos.Field<int>("IDPERSONAJE") == id
                           select datos;
            var row = consulta.First();
            Personaje personaje = new Personaje
            {
                IdPersonaje = id,
                NomPersonaje = row.Field<string>("PERSONAJE"),
                Imagen = row.Field<string>("IMAGEN")
            };
            return personaje;
        }

        public void UpdatePersonaje(Personaje personaje)
        {
            string sql = "UPDATE PERSONAJES SET PERSONAJE=:PERSONAJE, IMAGEN=:IMAGEN" +
                " WHERE IDPERSONAJE=:IDPERSONAJE";
            OracleParameter paramPersonaje = new OracleParameter(":PERSONAJE", personaje.NomPersonaje);
            this.com.Parameters.Add(paramPersonaje);
            OracleParameter paramImagen = new OracleParameter(":IMAGEN", personaje.Imagen);
            this.com.Parameters.Add(paramImagen);
            OracleParameter paramId = new OracleParameter(":IDPERSONAJE", personaje.IdPersonaje);
            this.com.Parameters.Add(paramId);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeletePersonaje(int id)
        {
            string sql = "DELETE FROM PERSONAJES WHERE IDPERSONAJE = :IDPERSONAJE";
            OracleParameter paramIdPersonaje = new OracleParameter(":IDPERSONAJE", id);
            this.com.Parameters.Add(paramIdPersonaje);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public List<Personaje> FiltrarPersonajes(string nombre)
        {
            var consulta = from datos in this.tablaPersonajes.AsEnumerable()
                           where datos.Field<string>("PERSONAJE").ToUpper()
                           == nombre.ToUpper()
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            List<Personaje> personajes = new List<Personaje>();
            foreach (var row in consulta)
            {
                Personaje personaje = new Personaje
                {
                    IdPersonaje = row.Field<int>("IDPERSONAJE"),
                    NomPersonaje = row.Field<string>("PERSONAJE"),
                    Imagen = row.Field<string>("IMAGEN")
                };
                personajes.Add(personaje);
            }
            return personajes;
        }

    }
}
