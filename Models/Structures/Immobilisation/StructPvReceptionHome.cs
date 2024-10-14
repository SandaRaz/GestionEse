using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructPvReceptionHome
    {
        public Immobilier immobilier = new Immobilier();
        public List<EtatImmobilier> etatImmobiliers = new List<EtatImmobilier>();

        public StructPvReceptionHome(string idBonDeLivraison)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            string idImmobilier = BonDeLivraison.GetIdImmobilier(cnx, idBonDeLivraison);
            immobilier = Immobilier.getImmobilier(cnx, idImmobilier) ?? throw new Exception($"ID IMMOBILIER {idImmobilier} NOT FOUND IN DATABASE");

            etatImmobiliers = EtatImmobilier.getEtatImmobiliers(cnx);

            cnx.Close();
        }
    }
}
