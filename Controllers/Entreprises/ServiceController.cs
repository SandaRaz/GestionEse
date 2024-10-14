using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using SIProject.Models.Entreprises.Commerce;

namespace SIProject.Controllers.Entreprises
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUnites(string idarticle)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            UniteDetaillee[]? unites = null;
            try
            {
                Article? article = Article.getArticle(cnx, idarticle);
                if (article != null)
                {
                    unites = article.getUnitesEquivalences(cnx);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n " + e);
            }

            cnx.Close();

            return Json(unites);
        }

        [HttpGet]
        public JsonResult GetPosts(string idDept)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            List<Poste> posts = new List<Poste>();
            try
            {
                posts = Poste.getPostes(cnx, idDept);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n " + e);
            }

            cnx.Close();

            return Json(posts);
        }
    }
}
