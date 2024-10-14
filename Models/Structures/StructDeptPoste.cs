using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using System.Text.Json;

namespace SIProject.Models.Structures
{
    public class StructDeptPoste
    {
        public Dept dept;
        public List<Poste> postes;

        public StructDeptPoste(string jsonDept)
        {
            dept = JsonSerializer.Deserialize<Dept>(jsonDept) ?? new Dept();

            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            postes = Poste.getPostes(cnx, dept.Id);

            cnx.Close();
        }
    }
}
