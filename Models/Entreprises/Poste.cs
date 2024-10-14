using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises
{
    [Serializable]
    public class Poste
    {
        private string _id = ""; public string Id { get { return _id; } set { _id = value; } }
        private string _nom= ""; public string Nom { get { return _nom; } set { _nom = value; } }
        private string _idDept = ""; public string IdDept { get { return _idDept; } set { _idDept = value; } }
        private int _hierarchie; public int Hierarchie { get { return _hierarchie; } set { _hierarchie = value; } }
    
        public Poste() { }
        public Poste(string id,string nom,string idDept,int hierarchie)
        {
            Id = id;
            Nom = nom;
            IdDept = idDept;
            Hierarchie = hierarchie;
        }
        public Poste(string nom, string idDept, int hierarchie)
        {
            Nom = nom;
            IdDept = idDept;
            Hierarchie = hierarchie;
        }

        public static bool nomExisteDeja(NpgsqlConnection cnx, string idDept, string nom)
        {
            if (nom == null)
            {
                throw new Exception("Le nom en entré est null");
            }
            bool existe = false;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM poste WHERE iddept=@iddept AND nom=@nom";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@iddept", idDept);
                command.Parameters.AddWithValue("@nom", nom);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        existe = true;
                    }
                }
            }

            if (closed)
            {
                cnx.Open();
            }
            return existe;
        }

        public int CreatePoste(NpgsqlConnection cnx)
        {
            int affectedRow = 0;
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "INSERT INTO poste(id,nom,iddept,hierarchie) " +
                "VALUES(@id,@nom,@iddept,@hierarchie)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", Connex.createId(cnx, "poste_sequence", "POS", 7));
                command.Parameters.AddWithValue("@nom", this.Nom);
                command.Parameters.AddWithValue("@iddept", this.IdDept);
                command.Parameters.AddWithValue("@hierarchie", this.Hierarchie);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Open();
            }
            return affectedRow;
        }

        public static int DeletePoste(NpgsqlConnection cnx, string idPoste)
        {
            int affectedRow = 0;
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "DELETE FROM poste WHERE id=@id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", idPoste);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Open();
            }
            return affectedRow;
        }

        public static Poste? getPoste(NpgsqlConnection cnx, string idPoste)
        {
            Poste? poste = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM poste WHERE id=@idposte";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idposte", idPoste);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            poste = new Poste(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                                Convert.ToString(reader["iddept"]), Convert.ToInt32(reader["hierarchie"]));
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET DEPT HAS 0 ROWS");
                    }
                }
            }

            if (closed)
            {
                cnx.Open();
            }
            return poste;
        }

        public static List<Poste> getPostes(NpgsqlConnection cnx, string idDept)
        {
            List<Poste> postes = new List<Poste>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM poste WHERE iddept=@iddept";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@iddept", idDept);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Poste poste = null;
                        while (reader.Read())
                        {
                            poste = new Poste(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                                Convert.ToString(reader["iddept"]), Convert.ToInt32(reader["hierarchie"]));
                            
                            postes.Add(poste);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET DEPT HAS 0 ROWS");
                    }
                }
            }

            if (closed)
            {
                cnx.Open();
            }
            return postes;
        }

        public List<Emp> getEmployes(NpgsqlConnection cnx)
        {
            List<Emp> employes = new List<Emp>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM emp WHERE id=@idposte";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idposte", this.Id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Emp? emp = null;
                        while (reader.Read())
                        {
                            emp = new Emp(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]), Convert.ToString(reader["prenoms"]),
                                Convert.ToDateTime(reader["datenaissance"]), Convert.ToString(reader["genre"]), Convert.ToString(reader["adresse"]),
                                Convert.ToString(reader["email"]), Convert.ToString(reader["contact"]), Convert.ToString(reader["photoFormat"]),
                                Convert.ToString(reader["photoName"]), Convert.ToString(reader["idposte"]));

                            employes.Add(emp);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET DEPT HAS 0 ROWS");
                    }
                }
            }

            if (closed)
            {
                cnx.Open();
            }
            return employes;
        }
    }
}
