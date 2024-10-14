using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Commerce;

namespace SIProject.Models.Structures.Commerce
{
    public class StructArticle
    {
        public List<Unite> unites;

        public StructArticle()
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            unites = Unite.getUnites(cnx);

            cnx.Close();
        }
    }
}
