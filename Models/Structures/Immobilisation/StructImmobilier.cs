using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Commerce;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructImmobilier
    {
        public List<CategorieImmo> categories = new List<CategorieImmo>();
        public CategorieImmo categorieImmo = new CategorieImmo();
        public List<Immobilier> immobiliers = new List<Immobilier>();
        public List<EtatImmobilier> etatImmobiliers = new List<EtatImmobilier>();
        public List<Fournisseur> fournisseurs = new List<Fournisseur>();

        public List<ProformaImmo> proformaImmos = new List<ProformaImmo>();
        public ProformaImmo proformaImmo = new ProformaImmo();

        public List<BonDeCommande> bonDeCommandes = new List<BonDeCommande>();
        public List<BonDeLivraison> bonDeLivraisons = new List<BonDeLivraison>();
        public List<PvReception> pvReceptions = new List<PvReception>();
        public List<DetailImmobilier> immobiliersAPVR = new List<DetailImmobilier>(); 

        public StructImmobilier()
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            categories = CategorieImmo.getCategorieImmo(cnx);
            etatImmobiliers = EtatImmobilier.getEtatImmobiliers(cnx);

            cnx.Close();
        }
        public StructImmobilier(string idCategorieImmo)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            categorieImmo = CategorieImmo.getCategorieImmo(cnx, idCategorieImmo) ?? throw new Exception("IdCategorieImmo non trouve ou Session expiree");
            immobiliers = Immobilier.getImmobiliers(cnx, idCategorieImmo);
            etatImmobiliers = EtatImmobilier.getEtatImmobiliers(cnx);
            fournisseurs = Fournisseur.getFournisseurs(cnx);

            proformaImmos = ProformaImmo.GetProformaImmos(cnx, idCategorieImmo, 0);
            bonDeCommandes = BonDeCommande.GetBonDeCommandes(cnx, idCategorieImmo, 0);
            bonDeLivraisons = BonDeLivraison.GetBonDeLivraisons(cnx, idCategorieImmo, 0);
            pvReceptions = PvReception.GetPvReceptions(cnx, idCategorieImmo, 0);
            immobiliersAPVR = DetailImmobilier.GetDetailImmobiliers(cnx, idCategorieImmo, 1);

            cnx.Close();
        }

        public StructImmobilier(string idCategorieImmo, string idProforma)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            categorieImmo = CategorieImmo.getCategorieImmo(cnx, idCategorieImmo) ?? throw new Exception("IdCategorieImmo non trouve ou Session expiree");
            proformaImmo = ProformaImmo.GetProformaImmo(cnx, idProforma) ?? throw new Exception($"IdProforma {idProforma} non trouvee");

            cnx.Close();
        }

        public EtatImmobilier GetEtat(string idEtatImmobilier)
        {
            EtatImmobilier etat = this.etatImmobiliers.Find(e => e.Id == idEtatImmobilier) ?? throw new Exception($"IdEtatImmobilier {idEtatImmobilier} does not exist in List");
            return etat;
        }
    }
}
