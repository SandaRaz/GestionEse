using SIProject.Models.Entreprises;
using System.Text.Json;

namespace SIProject.Models.Structures
{
    public class StructDeptMenu
    {
        public Dept dept;

        public StructDeptMenu(string jsonDept)
        {
            dept = JsonSerializer.Deserialize<Dept>(jsonDept);
        }

    }
}
