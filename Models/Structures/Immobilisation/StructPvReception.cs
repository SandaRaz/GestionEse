using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructPvReception
    {
        public PvReception pvReception = new PvReception();
        public Immobilier immobilier = new Immobilier();
        public List<EtatImmobilier> etatImmobiliers = new List<EtatImmobilier>();

        public StructPvReception(string idPvReception)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            pvReception = PvReception.GetPvReception(cnx, idPvReception) ?? throw new Exception($"ID PVRECEPTION {idPvReception} NOT FOUND IN DATABASE");
            immobilier = Immobilier.getImmobilier(cnx, pvReception.IdImmobilier) ?? throw new Exception($"ID IMMOBILIER {pvReception.IdImmobilier} NOT FOUND IN DATABASE");
            etatImmobiliers = EtatImmobilier.getEtatImmobiliers(cnx);

            cnx.Close();
        }

        public EtatImmobilier GetEtat(string idEtatImmobilier)
        {
            EtatImmobilier etat = this.etatImmobiliers.Find(e => e.Id == idEtatImmobilier) ?? throw new Exception($"IdEtatImmobilier {idEtatImmobilier} does not exist in List");
            return etat;
        }
    }
}
