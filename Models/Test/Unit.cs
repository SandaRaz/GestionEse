using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Test
{
    public class Unit
    {
        public static void main(string[] args)
        {
            Console.WriteLine("... EXECUTION DE MAIN ...");
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            cnx.Close();
        }
    }
}
