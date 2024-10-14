using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Shared;
using System.Text.Json;

namespace SIProject.Models.Entreprises.User
{
    public class Authentification
    {
        private int _id; public int Id { get { return _id; } set { _id = value; } }
        private string _idEse=""; public string IdEse { get { return _idEse; } set { _idEse = value; } }
        private string _mdp = ""; public string Mdp { get { return _mdp; } set { _mdp = value; } }
        
        public Authentification() { }
        public Authentification(int id,string idEse,string mdp)
        {
            Id = id;
            IdEse = idEse;
            Mdp = mdp;
        }
        public Authentification(string idEse, string mdp)
        {
            IdEse = idEse;
            Mdp = mdp;
        }

        public int saveAuthentification(NpgsqlConnection cnx, string idEse, string mdp)
        {
            int affectedRow = 0;

            if (mdp == null)
            {
                throw new Exception("Veuillez entrez votre mot de passe");
            }
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "INSERT INTO Authentification(id,idese,mdp) VALUES(default,@idese,@mdp)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idese", idEse);
                command.Parameters.AddWithValue("@mdp", Global.getMd5Hash(mdp));

                affectedRow = command.ExecuteNonQuery();
                if (affectedRow == 0)
                {
                    throw new Exception("FAILED TO SAVE ENTERPRISE PASSWORD");
                }
            }

            if (closed)
            {
                cnx.Open();
            }
            return affectedRow;
        }

        public static bool hasAccessPrivilege(string jsonLogged, string[] codeDepts,int limit)
        {
            Login logged = JsonSerializer.Deserialize<Login>(jsonLogged) ?? throw new Exception("LOGIN VALUE IN JSON IS NULL");
            Console.WriteLine($"Logged => codeDept:{logged.CodeDept}, hierachie:{logged.Hiercarchie}. Required => codeDepts: {string.Join(",", codeDepts)} hierarchie:{limit} ");
            bool validCodeDept = codeDepts.Contains(logged.CodeDept);
            Console.WriteLine($"Valid Code Dept: {validCodeDept}");
            if ((logged.Hiercarchie <= limit) && validCodeDept)
            {
                Console.WriteLine("Logged User Has Privilege for this Action");
                return true;
            }
            return false;
        }
    }
}
