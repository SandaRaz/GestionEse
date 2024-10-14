using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class EtatImmobilier
    {
        private string _id = "";
        private string _etat = "";
        private int _valeur;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }
        public int Valeur
        {
            get { return _valeur; }
            set { _valeur = value; }
        }

        public EtatImmobilier(string id, string etat, int valeur)
        {
            Id = id;
            Etat = etat;
            Valeur = valeur;
        }
        public EtatImmobilier(string etat, int valeur)
        {
            Etat = etat;
            Valeur = valeur;
        }

        public static List<EtatImmobilier> getEtatImmobiliers(NpgsqlConnection cnx)
        {
            List<EtatImmobilier> etatImmobilers = new List<EtatImmobilier>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM EtatImmobilier";
            using(NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        EtatImmobilier? etatImmobilier = null;
                        while (reader.Read())
                        {
                            etatImmobilier = new EtatImmobilier(reader.GetString(0), reader.GetString(1), reader.GetInt32(2));
                            etatImmobilers.Add(etatImmobilier);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET ETATIMMOBILIERS HAS'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return etatImmobilers;
        }

        public static EtatImmobilier? getEtatImmobilier(NpgsqlConnection cnx, string id)
        {
            EtatImmobilier? etatImmobilier = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM EtatImmobilier WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            etatImmobilier = new EtatImmobilier(reader.GetString(0), reader.GetString(1),reader.GetInt32(2));
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET ETATIMMOBILIER HAS'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return etatImmobilier;
        }
    }
}
