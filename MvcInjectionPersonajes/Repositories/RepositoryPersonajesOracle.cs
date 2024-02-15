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
                "(:p_idpersonaje, :p_nompersonaje, :p_imagen)";
            OracleParameter paramIdPersonaje = new OracleParameter(":p_idpersonaje", personaje.IdPersonaje);
            this.com.Parameters.Add(paramIdPersonaje);
            OracleParameter paramNomPersonaje = new OracleParameter(":p_nompersonaje", personaje.NomPersonaje);
            this.com.Parameters.Add(paramNomPersonaje);
            OracleParameter paramImagen = new OracleParameter(":p_imagen", personaje.Imagen);
            this.com.Parameters.Add(paramImagen);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
