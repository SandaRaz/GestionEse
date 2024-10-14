using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using System.Text.Json;

namespace SIProject.Controllers.Entreprises
{
    public class DeptController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AjoutHome()
        {
            return View();
        }

        public IActionResult CreateDept(string idEse, string nom, string code, DateTime dateCreation, string idResp)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            if(Dept.nomExisteDeja(cnx, idEse, nom))
            {
                ViewData["NameError"] = true;
                cnx.Close();
                return View("AjoutHome");
            }
            Dept? dept = null;
            try
            {
                dept = new Dept(nom, code, dateCreation, idEse, idResp);
                dept.CreateDepartement(cnx, dept);
            }
            catch (Exception e)
            {
                ViewData["InsertError"] = true;
                Console.WriteLine("EXCEPTION: " + e);
                cnx.Close();
                return View("AjoutHome");
            }
            cnx.Close();
            if (dept == null)
            {
                ViewData["InsertError"] = true;
                return View("AjoutHome");
            }
            else
            {
                ViewData["InsertSuccess"] = true;
                return View("AjoutHome");
            }
        }

        public IActionResult DeleteDept(string idDept)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Dept.Delete(cnx, idDept);

            cnx.Close();

            return View("AjoutHome");
        }

        public IActionResult Menu(string idDept)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Dept dept = Dept.getDepartement(cnx, idDept);
            string jsonStringDept = JsonSerializer.Serialize(dept);

            HttpContext.Session.SetString("deptMenuObject", jsonStringDept);

            cnx.Close();

            return View();
        }

        /* ------------- POSTE -------------- */

        public IActionResult AjoutPoste(string idDept)
        {
            ViewBag.idDept = idDept;
            return View();
        }

        public IActionResult CreatePoste(string idDept, string nom,string hierarchie)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Poste? newposte = null;
            try
            {
                if (Poste.nomExisteDeja(cnx, idDept, nom))
                {
                    ViewData["NameError"] = true;
                    cnx.Close();

                    return View("AjoutPoste");
                }

                newposte = new Poste(nom, idDept, Convert.ToInt32(hierarchie));
                newposte.CreatePoste(cnx);
            }
            catch (Exception e)
            {
                ViewData["InsertError"] = true;
                Console.WriteLine("EXCEPTION: " + e);
                cnx.Close();

                return View("AjoutPoste");
            }

            if (newposte == null)
            {
                ViewData["InsertError"] = true;
                return View("AjoutPoste");
            }
            else
            {
                ViewData["InsertSuccess"] = true;
                return View("AjoutPoste");
            }
        }

        [HttpGet]
        public JsonResult getAllPostes(string idDept)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            List<Poste> postes = Poste.getPostes(cnx, idDept);

            cnx.Close();

            return Json(postes);
        }

        public IActionResult DeletePoste(string idPoste)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            using(NpgsqlTransaction transaction = cnx.BeginTransaction())
            {
                try
                {
                    Poste? poste = Poste.getPoste(cnx, idPoste);
                    if (poste != null)
                    {
                        List<Emp> emps = poste.getEmployes(cnx);
                        foreach (Emp emp in emps)
                        {
                            emp.updatePoste(cnx, "INDEF05");
                        }
                        Poste.DeletePoste(cnx, idPoste);
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine("ERROR: \n" + e);
                }
                finally
                {
                    if(transaction.Connection != null)
                    {
                        transaction.Commit();
                    }
                }
            }

            cnx.Close();
            return View("AjoutPoste");
        }

        /* ---------------------------------- */
    }
}
