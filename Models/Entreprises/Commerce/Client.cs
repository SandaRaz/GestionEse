using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce; 
 
 
public class Client {
	string id; 
	string nom; 
	string mail; 
	string tel; 
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

    public Client(string id, string nom, string mail, string tel)
    {
        Id = id;
        Nom = nom;
        Mail = mail;
        Tel = tel;
    }
    public Client(string nom, string mail, string tel)
    {
        Nom = nom;
        Mail = mail;
        Tel = tel;
    }

    public static Client? getClient(NpgsqlConnection cnx, string idclient)
    {
        Client? client = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM client WHERE id=@idclient";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@id", idclient);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        client = new Client(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["mail"]), Convert.ToString(reader["tel"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER GET CLIENT HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return client;
    }

    public static Client? getClientByMail(NpgsqlConnection cnx, string mail)
    {
        Client? client = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM client WHERE mail=@mail";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@mail", mail);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        client = new Client(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["mail"]), Convert.ToString(reader["tel"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER GET CLIENT HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return client;
    }

}