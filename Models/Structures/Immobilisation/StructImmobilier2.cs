using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Immobilisation;

namespace SIProject.Models.Structures.Immobilisation
{
    public class StructImmobilier2
    {
        public BonDeCommande bonDeCommande = new BonDeCommande();

        public StructImmobilier2(string idBonDeCommande)
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            bonDeCommande = BonDeCommande.GetBonDeCommande(cnx, idBonDeCommande) ?? throw new Exception("idBonDeCommande not found");

            cnx.Close();
        }
    }
}
