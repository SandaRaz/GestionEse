using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructPvUtilisation
    {
        public CategorieImmo categorieImmo = new CategorieImmo();
        public List<EtatImmobilier> etatImmobiliers = new List<EtatImmobilier>();

        public StructPvUtilisation(string idCategorieImmo)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            categorieImmo = CategorieImmo.getCategorieImmo(cnx, idCategorieImmo) ?? throw new Exception("IDCATEGORIEIMMO NOT FOUND");
            etatImmobiliers = EtatImmobilier.getEtatImmobiliers(cnx);

            cnx.Close();
        }
    }
}
