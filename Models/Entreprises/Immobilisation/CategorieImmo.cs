using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class CategorieImmo
    {
        private string _id = "";
        private string _nom = "";
        private string _label = "";

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public CategorieImmo()
        {
        }

        public CategorieImmo(string id, string nom, string label)
        {
            Id = id;
            Nom = nom;
            Label = label;
        }

        public CategorieImmo(string nom, string label)
        {
            Nom = nom;
            Label = label;
        }

        public static List<CategorieImmo> getCategorieImmo(NpgsqlConnection cnx)
        {
            List<CategorieImmo> categories = new List<CategorieImmo>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM CategorieImmo";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        CategorieImmo? categorie = null;
                        while (reader.Read())
                        {
                            categorie = new CategorieImmo(reader.GetString(0), reader.GetString(1), reader.GetString(2));

                            categories.Add(categorie);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET CATEGORIESIMMO HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return categories;
        }

        public static CategorieImmo? getCategorieImmo(NpgsqlConnection cnx, string id)
        {
            CategorieImmo? categorie = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM CategorieImmo WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            categorie = new CategorieImmo(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET CATEGORIEIMMO BY ID HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return categorie;
        }

    }
}
