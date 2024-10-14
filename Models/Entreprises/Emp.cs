using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises
{
    public class Emp
    {
        private string _id = ""; public string Id { get { return _id; } set { _id = value; } }
        private string _nom = ""; public string Nom { get { return _nom; } set { _nom = value; } }
        private string _prenoms; public string Prenoms { get { return _prenoms != null ? _prenoms : ""; } set { _prenoms = value; } }
        private DateTime _datenaissance; public DateTime DateNaissance { get { return _datenaissance; } set { _datenaissance = value; } }
        private string _genre = ""; public string Genre { get { return _genre; } set { _genre = value; } }
        private string _adresse; public string Adresse { get { return _adresse != null ? _adresse : ""; } set { _adresse = value; } }
        private string _email; public string Email { get { return _email != null ? _email : ""; } set { _email = value; } }
        private string _contact; public string Contact { get { return _contact != null ? _contact : ""; } set { _contact = value; } }
        private string _photoFormat; public string PhotoFormat { get { return _photoFormat != null ? _photoFormat : ""; } set { _photoFormat = value; } }
        private string _photoName; public string PhotoName { get { return _photoName != null ? _photoName : ""; } set { _photoName = value; } }
        private string _idPoste; public string IdPoste { get { return _idPoste != null ? _idPoste : ""; } set { _idPoste = value; } }
    
        public Emp() { }
        public Emp(string id,string nom,string prenoms, DateTime dateNaissance,string genre,string adresse,
            string email,string contact,string photoFormat, string photoName, string idPoste)
        {
            Id = id;
            Nom = nom;
            Prenoms = prenoms;
            DateNaissance = dateNaissance;
            Genre = genre;
            Adresse = adresse;
            Email = email;
            Contact = contact;
            PhotoFormat = photoFormat;
            PhotoName = photoName;
            IdPoste = idPoste;
        }
        public Emp(string nom, string prenoms, DateTime dateNaissance, string genre, string adresse,
            string email, string contact, string photoFormat, string photoName, string idPoste)
        {
            Nom = nom;
            Prenoms = prenoms;
            DateNaissance = dateNaissance;
            Genre = genre;
            Adresse = adresse;
            Email = email;
            Contact = contact;
            PhotoFormat = photoFormat;
            PhotoName = photoName;
            IdPoste = idPoste;
        }

        public int CreateEmp(NpgsqlConnection cnx)
        {
            int affectedRow = 0;
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "INSERT INTO emp(id,nom,prenoms,datenaissance,genre,adresse,email,contact,photoFormat," +
                "photoName,idposte) VALUES(@id,@nom,@prenoms,@datenaissance,@genre,@adresse,@email,@contact,@photoFormat," +
                "@photoName,@idposte)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", Connex.createId(cnx, "emp_sequence", "EMP", 7));
                command.Parameters.AddWithValue("@nom", this.Nom);
                command.Parameters.AddWithValue("@prenoms", this.Prenoms);
                command.Parameters.AddWithValue("@datenaissance", this.DateNaissance);
                command.Parameters.AddWithValue("@genre", this.Genre);
                command.Parameters.AddWithValue("@adresse", this.Adresse);
                command.Parameters.AddWithValue("@email", this.Email);
                command.Parameters.AddWithValue("@contact", this.Contact);
                command.Parameters.AddWithValue("@photoFormat", this.PhotoFormat);
                command.Parameters.AddWithValue("@photoName", this.PhotoName);
                command.Parameters.AddWithValue("@idposte", this.IdPoste);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Open();
            }
            return affectedRow;
        }

        public int updatePoste(NpgsqlConnection cnx, string newIdPoste)
        {
            int affectedRow = 0;
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "UPDATE emp SET idposte=@idposte WHERE id=@id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", this.Id);
                command.Parameters.AddWithValue("@idposte", newIdPoste);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Open();
            }
            return affectedRow;
        }
    }
}
