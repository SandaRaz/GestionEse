using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructAmortissement
    {
        public CategorieImmo categorieImmo = new CategorieImmo();
        public List<DetailImmobilier> immobiliers = new List<DetailImmobilier>();

        public StructAmortissement(string idCategorieImmo)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            categorieImmo = CategorieImmo.getCategorieImmo(cnx, idCategorieImmo) ?? throw new Exception("ID CATEGORIEIMMO NOT FOUND IN DATABASE");
            immobiliers = DetailImmobilier.GetDetailImmobiliers(cnx, idCategorieImmo, 2);

            cnx.Close();
        }
    }
}
