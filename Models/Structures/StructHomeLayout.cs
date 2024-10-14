using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using System.Text.Json;

namespace SIProject.Models.Structures
{
    public class StructHomeLayout
    {
        public Entreprise entreprise;
        public List<Dept> departements;

        public StructHomeLayout(string jsonStringEse)
        {
            this.entreprise = JsonSerializer.Deserialize<Entreprise>(jsonStringEse);
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            this.departements = this.entreprise.getListeDepartement(cnx);

            cnx.Close();
        }
    }
}
