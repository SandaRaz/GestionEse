using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;
using SIProject.Models.Entreprises.User;
using SIProject.Models.ViewModel;
using System.Text.Json;

namespace SIProject.Controllers.Entreprises
{
    public class ImmobilisationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Categories(string idCategorieImmo, string nomPage)
        {
            string view = "Categorie/" + nomPage;

            HttpContext.Session.SetString("idCategorieImmo", idCategorieImmo);
            return View(view);
        }

        public IActionResult QuitterCategorie()
        {
            HttpContext.Session.Remove("idCategorieImmo");
            return View("Index");
        }

        public IActionResult Achat()
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            string idCategorieImmo = HttpContext.Session.GetString("idCategorieImmo") ?? throw new Exception("IDCategorie non trouvee dans la Session");
            CategorieImmo categorieImmo = CategorieImmo.getCategorieImmo(cnx, idCategorieImmo) ?? throw new Exception("Get Categorie by idCategorie hasn't any row, idCategorie is not found in Database");
            cnx.Close();

            string view = "Categorie/" + categorieImmo.Nom;

            string jsonLoginEmp = HttpContext.Session.GetString("LoggedEmp") ?? "";
            if (!String.IsNullOrWhiteSpace(jsonLoginEmp))
            {
                Login? logged = JsonSerializer.Deserialize<Login>(jsonLoginEmp);
                if(logged != null)
                {
                    if(logged.Hiercarchie > 1)
                    {
                        return View(view);
                    }
                }
            }
            else
            {
                return View(view);
            }
            return View();
        }

        public IActionResult NouveauImmobilier(string nomImmobilisation, string idCategorieImmo, string idEse)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Immobilier immobilier = new Immobilier(nomImmobilisation, idCategorieImmo, idEse);
            int saved = 0;
            if(!Immobilier.nomExisteDeja(cnx, nomImmobilisation, idCategorieImmo))
            {
                saved = immobilier.save(cnx);
            }

            cnx.Close();
            if (saved <= 0)
            {
                ViewBag.erreurInsertion = true;
            }
            return View("Achat");
        }

        public IActionResult AjoutProforma(AjoutProformaImmoViewModel pvm)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            double quantite = pvm.quantite;

            ProformaImmo proforma = new ProformaImmo(pvm.idImmobilier, 1, pvm.prix, pvm.dateEmission,
                pvm.dateExpiration, pvm.idEtat, pvm.idFournisseur, pvm.fournisseur, pvm.reference, 0);
            for(int i=0; i<quantite; i++)
            {
                proforma.save(cnx);
            }

            cnx.Close();
            return View("Achat");
        } 

        public IActionResult BDCHome(string idProformaImmo)
        {
            ViewBag.idProformaImmo = idProformaImmo;

            return View();
        }
        public IActionResult AjoutBDC(string idProformaImmo,double prixHt,double tva,double prixTtc,
            DateTime dateCommande, DateTime dateLivraison)
        {
            BonDeCommande bdc = new BonDeCommande(idProformaImmo,prixHt,tva,prixTtc,dateCommande,dateLivraison,0);

            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            bool erreur = false;
            using(NpgsqlTransaction transaction = cnx.BeginTransaction())
            {
                try
                {
                    bdc.Save(cnx);
                    ProformaImmo.UpdateStatut(cnx, idProformaImmo, 1);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.Error.WriteLine("Error: " + e);
                    erreur = true;
                }
            }


            ViewBag.idProformaImmo = idProformaImmo;
            if (erreur)
            {
                ViewBag.erreurInsertion = true;
            }

            return View("BDCHome");
        }

        public IActionResult BDLHome(string idBonDeCommande)
        {
            ViewBag.idBonDeCommande = idBonDeCommande;

            return View();
        }

        public IActionResult AjoutBDL(AjoutBDLViewModel bvm)
        {
            BonDeLivraison bdl = new BonDeLivraison(bvm.idBonDeCommande, bvm.quantiteLivree, bvm.prix, bvm.frais,
                bvm.descriptions, bvm.observations, bvm.adresse, bvm.livreur, bvm.contactLivreur, bvm.dateLivraison, 0);

            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            bool erreur = false;
            using (NpgsqlTransaction transaction = cnx.BeginTransaction())
            {
                try
                {
                    bdl.Save(cnx);
                    BonDeCommande.UpdateStatut(cnx, bvm.idBonDeCommande, 1);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.Error.WriteLine("Error: " + e);
                    erreur = true;
                }
            }


            ViewBag.idBonDeCommande = bvm.idBonDeCommande;
            if (erreur)
            {
                ViewBag.erreurInsertion = true;
            }
            return View("BDLHome");
        }

        public IActionResult PVReceptionHome(string idBonDeLivraison)
        {
            ViewBag.idBonDeLivraison = idBonDeLivraison;

            return View();
        }

        public IActionResult AjoutPvReception(string idImmobilier, string idBonDeLivraison, DateTime dateReception,string numeroCompta,
            string marque,string modele,string descriptions,string idEtat,string recepteur)
        {
            PvReception pvReception = new PvReception(idImmobilier, idBonDeLivraison, dateReception, 
                numeroCompta, marque, modele, descriptions, idEtat, recepteur, 0);
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            bool erreur = false;
            using(NpgsqlTransaction transaction = cnx.BeginTransaction())
            {
                try
                {
                    pvReception.save(cnx);
                    BonDeLivraison.UpdateStatut(cnx, idBonDeLivraison, 1);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    Console.Error.WriteLine("Error: " + e);
                    erreur = true;
                }
            }

            cnx.Close();

            ViewBag.idBonDeLivraison = idBonDeLivraison;
            if (erreur)
            {
                ViewBag.erreurInsertion = true;
            }
            return View("PVReceptionHome");
        }

        public IActionResult PVRValidationPage(string idPvReception)
        {
            ViewBag.idPvReception = idPvReception;

            return View();
        }

        public IActionResult ValiderPvReception(string idPvReception)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            int updated = PvReception.UpdateStatut(cnx, idPvReception, 1);

            cnx.Close();

            ViewBag.idPvReception = idPvReception;
            if (updated <= 0)
            {
                ViewBag.erreurValidation = true;
            }
            

            return View("Achat");
        }

        public IActionResult Suivie()
        {
            return View();
        }

        public IActionResult PremiereUtilisation(string idPvReception)
        {
            ViewBag.idPvReception = idPvReception;

            return View();
        }

        public IActionResult AjoutPvUtilisation(string idPvReception, DateTime dateDebutUtilisation,
            string utilisateurActuel, string idEtat)
        {
            PvUtilisation pvUtilisation = new PvUtilisation(idPvReception, dateDebutUtilisation, utilisateurActuel, idEtat);
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            DateTime dateDerniere = PvReception.getDateDerniereUtilisation(cnx, idPvReception);
            if (dateDerniere > dateDebutUtilisation)
            {
                ViewBag.idPvReception = idPvReception;
                ViewBag.erreurInsertion = true;

                return View("PremiereUtilisation");
            }

            bool erreur = false;
            using(NpgsqlTransaction transaction = cnx.BeginTransaction())
            {
                try
                {
                    pvUtilisation.save(cnx);
                    PvReception.UpdateStatut(cnx, idPvReception, 2);

                    transaction.Commit();
                }catch(Exception e)
                {
                    transaction.Rollback();
                    erreur = true;
                    Console.Error.WriteLine("ERROR: " + e);
                }
            }

            cnx.Close();

            ViewBag.idPvReception = idPvReception;
            if (erreur)
            {
                ViewBag.erreurInsertion = true;
            }

            return View("Suivie");
        }

        public IActionResult MouvementImmo(string idPvReception)
        {
            ViewBag.idPvReception = idPvReception;

            if (HttpContext.Session.GetString("erreurInsertion") != null)
            {
                ViewBag.erreurInsertion = true;
                HttpContext.Session.Remove("erreurInsertion");
            }

            return View();
        }

        public IActionResult AjoutMvtUtilisation(string idPvReception, DateTime dateDebutUtilisation,
            string utilisateurActuel, string idEtat)
        {
            PvUtilisation pvUtilisation = new PvUtilisation(idPvReception, dateDebutUtilisation, utilisateurActuel, idEtat);
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            DateTime dateDerniere = PvReception.getDateDerniereUtilisation(cnx, idPvReception);
            if (dateDerniere > dateDebutUtilisation)
            {
                ViewBag.idPvReception = idPvReception;

                HttpContext.Session.SetString("erreurInsertion", "true");
                return RedirectToAction("MouvementImmo", new { idPvReception = idPvReception });
            }

            bool erreur = false;
            try
            {
                pvUtilisation.save(cnx);
                PvReception.UpdateStatut(cnx, idPvReception, 2);
            }
            catch (Exception e)
            {
                erreur = true;
                Console.Error.WriteLine("ERROR: " + e);
            }

            cnx.Close();

            ViewBag.idPvReception = idPvReception;
            if (erreur)
            {
                ViewBag.erreurInsertion = true;
            }

            return View("Suivie");
        }

        public IActionResult Amortissement()
        {


            return View();
        }

        public IActionResult AffichageAmortissement(string idPvReception)
        {
            ViewBag.idPvReception = idPvReception;

            return View();
        }

        public IActionResult GetAmortLineaire(string idPvReception, double annuiteAmortissement)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            List<Amortissement> amortissementLineaires = DetailImmobilier.getAmortissementLineaire(cnx, idPvReception, annuiteAmortissement);

            cnx.Close();

            ViewBag.idPvReception = idPvReception;
            ViewBag.amortissementLineaires = amortissementLineaires;

            return View("AffichageAmortissement");
        }
    }
}
