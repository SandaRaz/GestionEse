using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;

namespace SIProject.Models.Structures
{
    public class StructLoginHome
    {
        public List<Dept> depts;

        public StructLoginHome(string idEse)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Entreprise entreprise = new Entreprise().getEntreprise(cnx, idEse);
            depts = entreprise.getListeDepartement(cnx);

            cnx.Close();
        }
    }
}
