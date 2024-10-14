using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Commerce;

namespace SIProject.Models.Structures.Commerce
{
    public class StructStockage
    {
        public List<Article> articles;

        public StructStockage(string idese)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            articles = Article.getArticles(cnx, idese, 5);

            cnx.Close();
        }
    }
}
