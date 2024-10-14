using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Shared;

namespace SIProject.Models.Entreprises
{
    [Serializable]
    public class Dept
    {
        private string _id = ""; public string Id { get { return _id; } set { _id = value; } }
        private string _nom = ""; public string Nom { get { return _nom; } set { _nom= value; } }

        private string _code = ""; public string Code { get { return _code; } set { _code = value; } }
        private DateTime _dateCreation; public DateTime DateCreation { get { return _dateCreation; } set { _dateCreation = value; } }
        private string _idEse = ""; public string IdEse { get { return _idEse; } set { _idEse = value; } }
        private string _idResponsable = ""; public string IdResponsable { get { return _idResponsable!=null?_idResponsable:""; } set { _idResponsable = value; } }
    
        public Dept() { }
        public Dept(string id,string nom,string code,DateTime dateCreation,string idEse,string idResponsable)
        {
            Id = id;
            Nom = nom;
            Code = code;
            DateCreation = dateCreation;
            IdEse = idEse;
            IdResponsable = idResponsable;
        }
        public Dept(string nom, string code, DateTime dateCreation, string idEse, string idResponsable)
        {
            Nom = nom;
            Code = code;
            DateCreation = dateCreation;
            IdEse = idEse;
            IdResponsable = idResponsable;
        }

        public int CreateDepartement(NpgsqlConnection cnx, Dept dept)
        {
            if (dept == null)
            {
                throw new Exception("Dept passé en parametre de createEntreprise est null");
            }
            else
            {
                if (dept.IdEse == null || dept.Nom == null)
                {
                    throw new Exception("Un ou plusieurs des attribut de Dept est null");
                }
            }
            int affectedRow = 0;
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "INSERT INTO dept(id,nom,code,datecreation,idese,idresponsable) " +
                "VALUES(@id,@nom,@code,@datecreation,@idese,@idresponsable)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", Connex.createId(cnx, "dept_sequence", "DEP", 7));
                command.Parameters.AddWithValue("@nom", dept.Nom);
                command.Parameters.AddWithValue("@code", dept.Code);
                command.Parameters.AddWithValue("@datecreation", dept.DateCreation);
                command.Parameters.AddWithValue("@idese", dept.IdEse);
                command.Parameters.AddWithValue("@idresponsable", dept.IdResponsable);

                affectedRow = command.ExecuteNonQuery();
            }
            
            if (closed)
            {
                cnx.Open();
            }
            return affectedRow;
        }

        public static Dept? getDepartement(NpgsqlConnection cnx, string idDept)
        {
            Dept? dept = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM dept WHERE id=@iddept";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@iddept", idDept);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dept = new Dept(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                                Convert.ToString(reader["code"]), Convert.ToDateTime(reader["datecreation"]), 
                                Convert.ToString(reader["idese"]), Convert.ToString(reader["idresponsable"]));
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
            return dept;
        }

        public Dept? getDepartementByCode(NpgsqlConnection cnx, string code)
        {
            Dept? dept = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM dept WHERE code=@code";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@code", code);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dept = new Dept(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                                Convert.ToString(reader["code"]), Convert.ToDateTime(reader["datecreation"]),
                                Convert.ToString(reader["idese"]), Convert.ToString(reader["idresponsable"]));
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
            return dept;
        }

        public static bool nomExisteDeja(NpgsqlConnection cnx, string idEse,string nom)
        {
            if (nom==null)
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

            string sql = "SELECT * FROM dept WHERE idese=@idese AND nom=@nom";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idese", idEse);
                command.Parameters.AddWithValue("@nom", nom);
                using(NpgsqlDataReader reader = command.ExecuteReader())
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

        public static int Delete(NpgsqlConnection cnx, string idDept)
        {
            int deletedRow = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "DELETE FROM Dept WHERE id=@iddept";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@iddept", idDept);

                deletedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Open();
            }
            return deletedRow;
        }
    }
}
