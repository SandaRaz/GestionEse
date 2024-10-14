using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using SIProject.Models.Entreprises.Commerce;
using SIProject.Models.Entreprises.User;
using SIProject.Models.Entreprises.User.EmailService;
using SIProject.Models.ViewModel;
using System.Text.Json;

namespace SIProject.Controllers.Entreprises
{
    public class CommerceController : Controller
    {
        private readonly IEmailSender _emailSender;
        public CommerceController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }


        public IActionResult Index()
        {
            return View();
        }

    /* --------- HOME, FORM ---------- */
        public IActionResult ArticleHome()
        {
            return View();
        }

        public IActionResult AchatBesoinHome()
        {
            return View();
        }

        public IActionResult StockageHome()
        {
            return View();
        }

        public IActionResult ProformaHome()
        {
            return View();
        }

        public IActionResult ProformaAjout(string idMvt)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Mouvement mvt = Mouvement.getMouvement(cnx, idMvt);
            List<String> proformas = mvt.getIdProformas(cnx);

            cnx.Close();

            ViewBag.idmvt = idMvt;
            ViewBag.idarticle = mvt.Idarticle;
            ViewBag.proformas = proformas;
            return View();
        }

        public IActionResult ProformaDemande()
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Console.WriteLine("----------- Liste and save email from google gmail -----------");
            List<MimeMessage> mails = new List<MimeMessage>();

            try
            {
                mails = _emailSender.ReceiveEmails(new DateTime(2023, 11, 25), "Proforma");
            }
            catch(Exception e)
            {
                Console.WriteLine("*** Erreur de connexion lors de la recuperation d'email ***");
                Console.Error.WriteLine("DETAILS ERREUR: \n "+e);
            }

            foreach (MimeMessage mail in mails)
            {
                Mail newmail = new Mail(mail.MessageId, mail.Sender.Name, mail.Sender.Address, "",mail.To[0].Name, mail.Subject, mail.TextBody, mail.Date, 1);
                try
                {
                    newmail.save(cnx);
                }
                catch(Exception e)
                {
                    Console.Error.WriteLine("ERROR: \n "+e);
                    Console.WriteLine("*** Mail peut etre deja dans la base");
                }
            }

            cnx.Close();
            return View();
        }

        public IActionResult ShowProformaMail(string idmail)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Mail mail = null;
            try
            {
                mail = Mail.getMail(cnx, idmail);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n"+e);
                return View("ProformaDemande");
            }

            cnx.Close();

            ViewBag.showMail = mail;
            return View("ProformaDemande");
        }

        public IActionResult ProformaEnvoie()
        {
            return View();
        }

        public IActionResult EtatDeStock()
        {
            return View();
        }

    /* ------------------------------- */
    /* -------- CALL FUNCTION -------- */
        public IActionResult InsertProforma(string mailClient)
        {
            return View();
        }

        public IActionResult ProformaAddNew(string idMvt, string idArticle, string idFournisseur, double stockPossession,
           double prixUnitaire,string uniteQuantite, DateTime dateEmission, DateTime dateExpiration)
        {
            if (stockPossession==null && prixUnitaire==null)
            {
                ViewBag.infoNonComplet = "Remplissez les cases vides";
                return View("ProformaAjout");
            }
            if (idMvt == null)
            {
                Console.Error.WriteLine("ERROR: \n IDMVT EST NULL DANS ProformaAddNew de CommerceController");
            }

            /* --- Test access privilege --- */
            bool hasPrivilege = Authentification.hasAccessPrivilege(HttpContext.Session.GetString("LoggedEmp"), new string[] { "SC", "DFB" }, 3);
            if (!hasPrivilege)
            {
                ViewBag.accessPrivilegeError = "Vous ne pouvez pas effectuer cet action";
                ViewBag.idmvt = idMvt;
                ViewBag.idarticle = idArticle;
                return View("ProformaAjout");
            }

            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Mouvement mouvement = Mouvement.getMouvement(cnx, idMvt) ?? throw new Exception($"LE MOUVEMENT AVEC L'ID {idMvt} ET NULL OU PAS PRESENT DANS LA BASE");
            Client client = Client.getClientByMail(cnx, "25sandanirina@gmail.com") ?? throw new Exception("LE CLIENT AVEC L'EMAIL 25sandanirina@gmail.com ET NULL OU PAS PRESENT DANS LA BASE");
            string newidProforma = Connex.createId(cnx, "proforma_sequence", "PFM", 7);
            Proforma proforma = new Proforma(newidProforma, idArticle,stockPossession,prixUnitaire,uniteQuantite,dateEmission,dateExpiration, 1, idFournisseur, client.Id);

            mouvement.addProforma(cnx, proforma);

            cnx.Close();

            return RedirectToAction("ProformaAjout", "Commerce", new { idMvt = idMvt });
        }

        public async Task<IActionResult> SendProforma(SendProformaViewModel model)
        {
            if (!model.infosComplet())
            {
                ViewBag.infoNonComplet = "Remplissez les cases vides";
                return View("ProformaEnvoie");
            }
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Fournisseur fournisseur = Fournisseur.getFournisseurByMail(cnx, model.mailFournisseur) ?? throw new Exception("FOURNISSEUR NULL DANS SEND PROFORMAT");
            Client client = Client.getClientByMail(cnx, model.mailClient) ?? throw new Exception("CLIENT NULL DANS SEND PROFORMAT");

            Proforma proforma = new Proforma(model.idArticle,model.stockPossession,model.prixUnitaire,model.idUnite,
                model.dateEmission,model.dateExpiration,1, fournisseur.Id, client.Id);
            
            int affectedRow = proforma.save(cnx);
            if (affectedRow > 0)
            {
                proforma.sendMail(cnx, _emailSender, fournisseur.Nom, fournisseur.Mail, client.Nom, client.Mail);
            }

            cnx.Close();
            return RedirectToAction("ProformaEnvoie","Commerce");
        }

        public IActionResult AjouterNouveauArticle(string idEse, string nomArticle, int typeArticle, string unite, double valeurUnite)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            string newIdArticle = Connex.createId(cnx, "article_sequence", "ART", 7);
            Article article = new Article(newIdArticle, nomArticle, typeArticle, idEse);
            Uniteequivalence ue = new Uniteequivalence(newIdArticle, unite, valeurUnite);

            using(NpgsqlTransaction transaction = cnx.BeginTransaction())
            {
                try
                {
                    article.save(cnx);
                    ue.save(cnx);

                    transaction.Commit();
                }catch(Exception e)
                {
                    transaction.Rollback();
                    cnx.Close();
                    Console.Error.WriteLine("Erreur lors de la transaction insertion d'artice: " + e);
                }
            }

            cnx.Close();
            return View("ArticleHome");
        }

        public IActionResult StockerArticle(StockageViewModel model)
        {
            if (!model.infosComplet())
            {
                ViewBag.infoNonComplet = "Remplissez les cases vides";
                return View("ProformaEnvoie");
            }
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            Mouvement mouvement = new Mouvement(model.idArticle,model.quantite, 0, model.prixUnitaire, model.dates, 1);
            int affectedRow = mouvement.save(cnx);

            cnx.Close();
            return View("StockageHome");
        }

        public IActionResult EtatDeStockAffichage(string idEse, DateTime dateDebut, DateTime dateFin)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            string jsonEse = HttpContext.Session.GetString("Entreprise") ?? throw new Exception("Entreprise NULL DANS LE JSON DE ETATSTOCKAFFICHAGE de CommerceController");
            Entreprise ese = JsonSerializer.Deserialize<Entreprise>(jsonEse) ?? throw new Exception("DESERIALIZE NULL JSON ENTREPRISE DANS ETATSTOCKAFFICHAGE de Commerce Controller ");

            List<EtatDeStock> etatDeStocks = ese.getEtatsDeStocks(cnx, dateDebut, dateFin);

            cnx.Close();

            ViewBag.etatDeStocks = etatDeStocks;
            return View("EtatDeStock");
        }
    /* ------------------------------- */
    }
}
