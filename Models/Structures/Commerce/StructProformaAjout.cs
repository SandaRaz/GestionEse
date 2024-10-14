using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Commerce;

namespace SIProject.Models.Structures.Commerce
{
    public class StructProformaAjout
    {
        public Article article;
        public List<Fournisseur> fournisseurs;

        public StructProformaAjout(string idarticle)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            article = Article.getArticle(cnx, idarticle);
            fournisseurs = Fournisseur.getFournisseurs(cnx, "FOU9000");

            cnx.Close();
        }
    }
}
