using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class Immobilier
    {
        private string _id = "";
        private string _nom = "";
        private string _idCatImmo = "";
        private string _idEse = "";

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
        public string IdCatImmo
        {
            get { return _idCatImmo; }
            set { _idCatImmo = value; }
        }
        public string IdEse
        {
            get { return _idEse; }
            set { _idEse = value; }
        }

        public Immobilier()
        {
        }

        public Immobilier(string id, string nom, string idCatImmo, string idEse)
        {
            Id = id;
            Nom = nom;
            IdCatImmo = idCatImmo;
            IdEse = idEse;
        }

        public Immobilier(string nom, string idCatImmo, string idEse)
        {
            Nom = nom;
            IdCatImmo = idCatImmo;
            IdEse = idEse;
        }

        public int save(NpgsqlConnection cnx)
        {
            int insertedRow = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }
            string newId = this.Id;
            if (String.IsNullOrWhiteSpace(newId))
            {
                newId = Connex.createId(cnx, "immobilier_sequence", "IMM", 7);
            }

            string sql = "INSERT INTO Immobilier(id,nom,idcatimmo,idese) VALUES(@id,@nom,@idcatimmo,@idese)";
            using(NpgsqlCommand command = new NpgsqlCommand(sql, cnx)){
                command.Parameters.AddWithValue("@id", newId);
                command.Parameters.AddWithValue("@nom", this.Nom);
                command.Parameters.AddWithValue("@idcatimmo", this.IdCatImmo);
                command.Parameters.AddWithValue("@idese", this.IdEse);

                insertedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return insertedRow;
        }

        public static Immobilier? getImmobilier(NpgsqlConnection cnx, string id)
        {
            Immobilier? immobilier = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM Immobilier WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);

                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            immobilier = new Immobilier(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET IMMOBILIER BY ID HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return immobilier;
        }

        public static bool nomExisteDeja(NpgsqlConnection cnx, string nom, string idCategorie)
        {
            bool exist = false;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM Immobilier WHERE nom = @nom AND idcatimmo = @idcatimmo";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@nom", nom);
                command.Parameters.AddWithValue("@idcatimmo", idCategorie);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        exist = true;
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return exist;
        }

        public static List<Immobilier> getImmobiliers(NpgsqlConnection cnx, string idCategorie)
        {
            List<Immobilier> immobiliers = new List<Immobilier>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM Immobilier WHERE idcatimmo = @idcatimmo";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idcatimmo", idCategorie);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Immobilier? immobilier = null;
                        while (reader.Read())
                        {
                            immobilier = new Immobilier(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                            immobiliers.Add(immobilier);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET IMMOBILIER BY ID HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return immobiliers;
        }
    }
}
