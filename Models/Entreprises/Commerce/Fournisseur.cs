using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce; 
 
 
public class Fournisseur {
	string id; 
	string nom; 
	string mail; 
	string tel; 
	string responsable; 
    public string Id{ 
        get{ 
            return id; 
        } 
        set{ 
            id = value; 
        } 
    } 
 
    public string Nom{ 
        get{ 
            return nom; 
        } 
        set{ 
            nom = value; 
        } 
    } 
 
    public string Mail{ 
        get{ 
            return mail; 
        } 
        set{ 
            mail = value; 
        } 
    } 
 
    public string Tel{ 
        get{ 
            return tel; 
        } 
        set{ 
            tel = value; 
        } 
    } 
 
    public string Responsable{ 
        get{ 
            return responsable; 
        } 
        set{ 
            responsable = value; 
        } 
    }

    public Fournisseur(string id,string nom,string mail,string tel,string responsable)
    {
        Id = id;
        Nom = nom;
        Mail = mail;
        Tel = tel;
        Responsable = responsable;
    }
    public Fournisseur(string nom, string mail, string tel, string responsable)
    {
        Nom = nom;
        Mail = mail;
        Tel = tel;
        Responsable = responsable;
    }

    public static Fournisseur? getFournisseur(NpgsqlConnection cnx, string idfournisseur)
    {
        Fournisseur? fournisseur = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM fournisseur WHERE id=@idfournisseur";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idfournisseur", idfournisseur);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        fournisseur = new Fournisseur(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["mail"]), Convert.ToString(reader["tel"]), Convert.ToString(reader["responsable"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER GET FOURNISSEUR HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return fournisseur;
    }

    public static List<Fournisseur>? getFournisseurs(NpgsqlConnection cnx, string idsup)
    {
        List<Fournisseur> fournisseurs = new List<Fournisseur>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM fournisseur WHERE id>=@idsup";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idsup", idsup);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Fournisseur fournisseur = null;
                    while (reader.Read())
                    {
                        fournisseur = new Fournisseur(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["mail"]), Convert.ToString(reader["tel"]), Convert.ToString(reader["responsable"]));
                        fournisseurs.Add(fournisseur);
                    }
                }
                else
                {
                    Console.WriteLine("READER GET FOURNISSEUR HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return fournisseurs;
    }

    public static List<Fournisseur> getFournisseurs(NpgsqlConnection cnx)
    {
        List<Fournisseur> fournisseurs = new List<Fournisseur>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM fournisseur";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Fournisseur? fournisseur = null;
                    while (reader.Read())
                    {
                        fournisseur = new Fournisseur(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["mail"]), Convert.ToString(reader["tel"]), Convert.ToString(reader["responsable"]));
                        fournisseurs.Add(fournisseur);
                    }
                }
                else
                {
                    Console.WriteLine("READER GET FOURNISSEUR HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return fournisseurs;
    }

    public static Fournisseur? getFournisseurByMail(NpgsqlConnection cnx, string mail)
    {
        Fournisseur? fournisseur = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM fournisseur WHERE mail=@mail";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@mail", mail);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        fournisseur = new Fournisseur(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["mail"]), Convert.ToString(reader["tel"]), Convert.ToString(reader["responsable"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER GET FOURNISSEUR HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return fournisseur;
    }

}