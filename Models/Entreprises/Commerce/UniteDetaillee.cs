using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce
{
    [Serializable]
    public class UniteDetaillee : Uniteequivalence
    {
        string article;
        string unite;

        public string Article
        {
            get { return article; }
            set { article = value; }
        }

        public string Unite
        {
            get { return unite; }
            set { unite = value; }
        }

        public UniteDetaillee(string id, string idarticle, string article, string idunite, string unite, double valeur) : base(id, idarticle, idunite, valeur)
        {
            Article = article;
            Unite = unite;
        }

        public static Uniteequivalence? getUnite(NpgsqlConnection cnx, string id)
        {
            Uniteequivalence? unite = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM unitedetaillee WHERE id=@id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            unite = new UniteDetaillee(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                                Convert.ToString(reader["article"]), Convert.ToString(reader["idunite"]), 
                                Convert.ToString(reader["unite"]), Convert.ToDouble(reader["valeur"]));
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET UNITE HAS 0 ROWS");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return unite;
        }
    }
}
