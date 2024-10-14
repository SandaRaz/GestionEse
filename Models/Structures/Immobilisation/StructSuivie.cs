using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructSuivie
    {
        public CategorieImmo categorieImmo = new CategorieImmo();
        public List<DetailImmobilier> immobiliers = new List<DetailImmobilier>();
        public List<DetailImmobilier> immobiliersUtilisees = new List<DetailImmobilier>();

        public StructSuivie(string idCategorieImmo)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            categorieImmo = CategorieImmo.getCategorieImmo(cnx, idCategorieImmo) ?? throw new Exception("ID CATEGORIEIMMO NOT FOUND IN DATABASE");
            immobiliers = DetailImmobilier.GetDetailImmobiliers(cnx, idCategorieImmo, 1);
            immobiliersUtilisees = DetailImmobilier.GetDetailImmobiliers(cnx, idCategorieImmo, 2);

            cnx.Close();
        }
    }
}
