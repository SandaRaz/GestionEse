
namespace SIProject.Models.Entreprises.User
{
    public class Compte
    {
        private int _id; public int Id { get { return _id; } set { _id = value; } }
        private string _username = ""; public string Username { get { return _username; } set { _username = value; } }
        private string _email = ""; public string Email { get { return _email; } set { _email = value; } }
        private string _tel = ""; public string Tel { get { return _tel; } set { _tel = value; } }
        private string _mdp = ""; public string Mdp { get { return _mdp; } set { _mdp = value; } }
        private int _status; public int Status { get { return _status; } set { _status = value; } }

        public Compte() { }
        public Compte(int id, string username, string email, string tel, string mdp, int status)
        {
            Id = id;
            Username = username;
            Email = email;
            Tel = tel;
            Mdp = mdp;
            Status = status;
        }
        public Compte(string username, string email, string tel, string mdp, int status)
        {
            Username = username;
            Email = email;
            Tel = tel;
            Mdp = mdp;
            Status = status;
        }
    }
}
