using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Commerce;

namespace SIProject.Models.Structures.Commerce
{
    public class StructProformaHome
    {
        public List<MvtDetaillee> achatEnAttentes;
        public StructProformaHome()
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            achatEnAttentes = Mouvement.getAchatEnAttente(cnx);

            cnx.Close();
        }
    }
}
