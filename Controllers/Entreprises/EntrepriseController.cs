using Microsoft.AspNetCore.Mvc;

using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using SIProject.Models.Entreprises.User;
using SIProject.Models.ViewModel;
using System.Text.Json;

namespace SIProject.Controllers.Entreprises
{
    public class EntrepriseController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public EntrepriseController(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View("Home");
        }

        public IActionResult AjoutForm()
        {
            return View();
        }

        public async Task<IActionResult> Ajouter(AjoutEseViewModel model)
        {
            if (model.mdp != model.confirmmdp)
            {
                throw new Exception("ERREUR DE LA CONFIRMATION DE MOT DE PASSE");
            }
            int affectedRow = 0;

            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            string idEse = Connex.createId(cnx, "entreprise_sequence", "ENT", 7);
            var imgName = Guid.NewGuid().ToString() + Path.GetExtension(model.logo.FileName);
            Entreprise ese = new Entreprise(model.nom,model.email,model.objet,model.siege,model.chef,model.nif,
                model.numstat,model.status,model.datecreaction, model.logo.ContentType, imgName);

            using(NpgsqlTransaction transaction = cnx.BeginTransaction())
            {
                try
                {
                    affectedRow += ese.createEntreprise(cnx, ese, idEse);
                    affectedRow += new Authentification().saveAuthentification(cnx, idEse, model.mdp);

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    cnx.Close();
                    Console.WriteLine("Error stacktrace: \n"+e.StackTrace);
                    throw new Exception("ERREUR LORS DE L'ENREGISTREMENT: \n"+ e);
                }
                finally
                {
                    if(transaction.Connection != null)
                    {
                        transaction.Commit();
                        // ----------- UPLOAD FICHIER IMAGE(LOGO) -------------

                        var wwwRootPath = _hostEnvironment.WebRootPath;
                        Console.WriteLine("Web Root Path: "+wwwRootPath);
                        var imgUploadFolder = Path.Combine(wwwRootPath, "img/uploads/entreprise");
                        if (!Directory.Exists(imgUploadFolder))
                        {
                            Directory.CreateDirectory(imgUploadFolder);
                        }
                        var imgPath = Path.Combine(imgUploadFolder, imgName);
                        Console.WriteLine("Image Path: " + imgPath);
                        using (var fileStream = new FileStream(imgPath, FileMode.Create))
                        {
                            await model.logo.CopyToAsync(fileStream);
                        }
                        // ----------------------------------------------------
                    }
                }
            }
            cnx.Close();

            if(affectedRow == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("AjoutForm", "Entreprise");
            }
        }

        public IActionResult Liste()
        {
            HttpContext.Session.SetInt32("LoginError", 0);
            if (HttpContext.Session.GetString("Entreprise") == null)
            {
                NpgsqlConnection cnx = Connex.getConnection();
                cnx.Open();
                List<Entreprise> eses = new Entreprise().getListeEntreprises(cnx);
                cnx.Close();

                return View(eses);
            }
            else
            {
                return RedirectToAction("Home", "Entreprise");
            }
            
        }

        public IActionResult LoginHome(string idEse)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();
            Entreprise? ese = new Entreprise().getEntreprise(cnx, idEse);
            cnx.Close();

            if(ese!=null) Console.WriteLine("Entreprise choisit: " + ese.Nom);
            if (HttpContext.Session.GetInt32("LoginError") == 1)
            {
                ViewData["LoginError"] = true;
                HttpContext.Session.SetInt32("LoginError", 0);
            }
            return View(ese);
        }

        public IActionResult Login(string idEse, string idPoste, string mdp)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Entreprise? entreprise = null;
            Poste poste = null;
            Dept dept = null;
            try
            {
                entreprise = new Entreprise().Login(cnx, idEse, mdp);
                if (idPoste != null)
                {
                    poste = Poste.getPoste(cnx, idPoste);
                    dept = Dept.getDepartement(cnx, poste.IdDept);
                }
                else
                {
                    throw new Exception("IdPoste is null");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("EXCEPTION: "+e);
                cnx.Close();

                HttpContext.Session.SetInt32("LoginError", 1);
                return RedirectToAction("LoginHome", "Entreprise", new { idEse = idEse });
            }
            
            cnx.Close();
            if (entreprise == null)
            {
                Console.WriteLine("Diso ny mot de passe");
                HttpContext.Session.SetInt32("LoginError", 1);
                return RedirectToAction("LoginHome", "Entreprise", new { idEse = idEse});
            }
            else
            {
                HttpContext.Session.Remove("LoginError");

                // ---------- collecting every necessary values and put them in json ----------
                HttpContext.Session.SetString("idEntreprise", idEse);

                string jsonStringEntreprise = JsonSerializer.Serialize(entreprise);
                HttpContext.Session.SetString("Entreprise", jsonStringEntreprise);

                string jsonLoginEmp = JsonSerializer.Serialize(new Login(poste.IdDept, dept.Nom, dept.Code, poste.Hierarchie));
                HttpContext.Session.SetString("LoggedEmp", jsonLoginEmp);
                // ----------------------------------------------------------------------------
                Console.WriteLine("Login valide");
                return RedirectToAction("Home", "Entreprise");
            }
        }

        public IActionResult Home()
        {
            if (HttpContext.Session.GetString("Entreprise") == null)
            {
                return RedirectToAction("Liste", "Entreprise");
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("idEntreprise");
            HttpContext.Session.Remove("Entreprise");
            if (HttpContext.Session.GetString("deptMenuObject") != null)
            {
                HttpContext.Session.Remove("deptMenuObject");
            }

            return RedirectToAction("Liste", "Entreprise");
        }
    }
}
