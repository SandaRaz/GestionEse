using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructMouvementImmo
    {
        public PvUtilisation? dernierUtilisation = new PvUtilisation();
        public List<PvUtilisation> pvUtilisations = new List<PvUtilisation>();
        public List<EtatImmobilier> etatImmobiliers = new List<EtatImmobilier>();

        public StructMouvementImmo(string idPvReception)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            dernierUtilisation = PvReception.getLastPvUtilisation(cnx, idPvReception);
            pvUtilisations = PvUtilisation.getPvUtilisations(cnx, idPvReception);
            etatImmobiliers = EtatImmobilier.getEtatImmobiliers(cnx);

            cnx.Close();
        }
    }
}
