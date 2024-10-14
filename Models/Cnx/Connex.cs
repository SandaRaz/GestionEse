using Npgsql;

namespace SIProject.Models.Cnx
{
    public class Connex
    {
        public static NpgsqlConnection getConnection()
        {
            String connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=mdpprom15;Database=entreprise;";
            NpgsqlConnection cnx = new NpgsqlConnection(connectionString);
            return cnx;
        }

        public static String createId(NpgsqlConnection cnx, String nomsequence, String prefixe, int len)
        {
            String id = "";
            bool closed = false;
            if(cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            int seq = -1;
            String sql = "SELECT nextval(@nomsequence)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@nomsequence", nomsequence);
                Object tempSeq = command.ExecuteScalar() ?? throw new Exception($"SELECT nextval({nomsequence}) return NULL");
                seq = Convert.ToInt32(tempSeq);
            }

            if (seq == -1)
            {
                cnx.Close();
                throw new Exception($"Erreur lors de la recuperation de la valeur de la sequence: seq = -1");
            }
            else
            {
                int nbZero = len - (prefixe.Length + seq.ToString().Length);
                id = $"{prefixe}{new string('0', nbZero)}{seq}";
            }

            if (closed)
            {
                cnx.Close();
            }
            return id;
        }

        public static int getNextVal(NpgsqlConnection cnx, string nomSequence)
        {
            int nextval = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT nextval(@nomsequence)";
            using(NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@nomsequence", nomSequence);
                Object tempSeq = command.ExecuteScalar() ?? throw new Exception($"SELECT nextval({nomSequence}) return NULL");
                nextval = Convert.ToInt32(tempSeq);
            }

            if (closed)
            {
                cnx.Close();
            }
            return nextval;
        }

        public static string createSequenceId(String prefixe, int len, int sequence)
        {
            int nbZero = len - (prefixe.Length + sequence.ToString().Length);
            string id = $"{prefixe}{new string('0', nbZero)}{sequence}";

            Console.WriteLine("New ID >>> " + id);

            return id;
        }
    }
}
