using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce; 
 
 
public class Unite {
	string id; 
	string nom; 
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

    public Unite(string id,string nom)
    {
        Id = id;
        Nom = nom;
    }
    public Unite(string nom)
    {
        Nom = nom;
    }

    public int save(NpgsqlConnection cnx)
    {
        int affectedRow = 0;
        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        String sql = "INSERT INTO unite(id,nom) " +
            "VALUES(@id,@nom)";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            string newid = "";
            if (this.Id == null)
            {
                newid = Connex.createId(cnx, "unite_sequence", "UNI", 7);
            }
            else
            {
                newid = this.Id;
            }
            command.Parameters.AddWithValue("@id", newid);
            command.Parameters.AddWithValue("@nom", this.Nom);

            affectedRow = command.ExecuteNonQuery();
        }

        if (closed)
        {
            cnx.Close();
        }
        return affectedRow;
    }

    public static Unite? getUnite(NpgsqlConnection cnx, string id)
    {
        Unite? unite = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM unite WHERE id=@id";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@id", id);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        unite = new Unite(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER GET ARTICLE HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return unite;
    }

    public static List<Unite> getUnites(NpgsqlConnection cnx)
    {
        List<Unite> unites = new List<Unite>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM unite";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Unite? unite = null;
                    while (reader.Read())
                    {
                        unite = new Unite(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]));
                        unites.Add(unite);
                    }
                }
                else
                {
                    Console.WriteLine("READER GET UNITES HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return unites;
    }

}