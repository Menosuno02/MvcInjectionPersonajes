using MvcInjectionPersonajes.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcInjectionPersonajes.Repositories
{
    public class RepositoryPersonajesSQLClient : IRepositoryPersonajes
    {
        private DataTable tablaPersonajes;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryPersonajesSQLClient()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023;Encrypt=False";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaPersonajes = new DataTable();
            string sql = "SELECT * FROM PERSONAJES";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, this.cn);
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
                "(@IDPERSONAJE, @PERSONAJE, @IMAGEN)";
            this.com.Parameters.AddWithValue("@IDPERSONAJE", personaje.IdPersonaje);
            this.com.Parameters.AddWithValue("@PERSONAJE", personaje.NomPersonaje);
            this.com.Parameters.AddWithValue("@IMAGEN", personaje.Imagen);
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
                IdPersonaje = row.Field<int>("IDPERSONAJE"),
                NomPersonaje = row.Field<string>("PERSONAJE"),
                Imagen = row.Field<string>("IMAGEN")
            };
            return personaje;
        }

        public void UpdatePersonaje(Personaje personaje)
        {
            string sql = "UPDATE PERSONAJES SET PERSONAJE=@PERSONAJE,IMAGEN=@IMAGEN" +
                " WHERE IDPERSONAJE=@IDPERSONAJE";
            this.com.Parameters.AddWithValue("@PERSONAJE", personaje.NomPersonaje);
            this.com.Parameters.AddWithValue("@IMAGEN", personaje.Imagen);
            this.com.Parameters.AddWithValue("@IDPERSONAJE", personaje.IdPersonaje);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeletePersonaje(int id)
        {
            string sql = "DELETE FROM PERSONAJES WHERE IDPERSONAJE = @IDPERSONAJE";
            this.com.Parameters.AddWithValue("@IDPERSONAJE", id);
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
