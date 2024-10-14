namespace SIProject.Models.Entreprises.User
{
    [Serializable]
    public class Login
    {
        private string _idDept;
        private string _nomDept;
        private string _codeDept;
        private int _hierarchie;

        public String IdDept
        {
            get { return _idDept; }
            set { _idDept = value; }
        }
        public String NomDept
        {
            get { return _nomDept; }
            set { _nomDept = value; }
        }
        public String CodeDept
        {
            get { return _codeDept; }
            set { _codeDept = value; }
        }
        public int Hiercarchie
        {
            get { return _hierarchie; }
            set { _hierarchie = value; }
        }

        public Login(string idDept, string nomDept, string codeDept, int hiercarchie)
        {
            IdDept = idDept;
            NomDept = nomDept;
            CodeDept = codeDept;
            Hiercarchie = hiercarchie;
        }
    }
}
