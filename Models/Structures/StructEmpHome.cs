using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using System.Text.Json;

namespace SIProject.Models.Structures
{
    public class StructEmpHome
    {
        public List<Dept> depts;

        public StructEmpHome(string jsonEse)
        {
            Entreprise? ese = JsonSerializer.Deserialize<Entreprise>(jsonEse);
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            try
            {
                if (ese != null)
                {
                    depts = ese.getListeDepartement(cnx);
                }
                else
                {
                    Console.Error.WriteLine("ERROR: ENTREPRISE FROM JSON IS EMPTY/NULL");
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("ERROR: "+e);
            }

            cnx.Close();
        }
    }
}
