using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.Commerce;

namespace SIProject.Models.Structures.Commerce
{
    public class StructProformaDemande
    {
        public List<Mail> mails;

        public StructProformaDemande()
        {
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            mails = Mail.getMails(cnx, 1);

            cnx.Close();
        }
    }
}
