using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce; 
 
 [Serializable]
public class Uniteequivalence {
	string id; 
	string idarticle; 
	string idunite; 
	double valeur; 
    public string Id{ 
        get{ 
            return id; 
        } 
        set{ 
            id = value; 
        } 
    } 
 
    public string Idarticle{ 
        get{ 
            return idarticle; 
        } 
        set{ 
            idarticle = value; 
        } 
    } 
 
    public string Idunite{ 
        get{ 
            return idunite; 
        } 
        set{ 
            idunite = value; 
        } 
    } 
 
    public double Valeur{ 
        get{ 
            return valeur; 
        } 
        set{ 
            valeur = value; 
        } 
    } 

    public Uniteequivalence(string id,string idarticle,string idunite,double valeur)
    {
        Id = id;
        Idarticle = idarticle;
        Idunite = idunite;
        Valeur = valeur;
    }
    public Uniteequivalence(string idarticle, string idunite, double valeur)
    {
        Idarticle = idarticle;
        Idunite = idunite;
        Valeur = valeur;
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

        String sql = "INSERT INTO uniteequivalence(id,idarticle,idunite,valeur) " +
            "VALUES(@id,@idarticle,@idunite,@valeur)";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            string newid = "";
            if (this.Id == null)
            {
                newid = Connex.createId(cnx, "uniteequivalence_sequence", "UEQ", 7);
            }
            else
            {
                newid = this.Id;
            }
            command.Parameters.AddWithValue("@id", newid);
            command.Parameters.AddWithValue("@idarticle", this.Idarticle);
            command.Parameters.AddWithValue("@idunite", this.Idunite);
            command.Parameters.AddWithValue("@valeur", this.Valeur);

            affectedRow = command.ExecuteNonQuery();
        }

        if (closed)
        {
            cnx.Close();
        }
        return affectedRow;
    }

    public static Uniteequivalence? getUnite(NpgsqlConnection cnx, string id)
    {
        Uniteequivalence? unite = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM uniteequivalence WHERE id=@id";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@id", id);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        unite = new Uniteequivalence(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToString(reader["idunite"]), Convert.ToDouble(reader["valeur"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER GET UNITE HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return unite;
    }

    public static double ConvertirQuantite(Uniteequivalence principale, double valeur, Uniteequivalence uniteValeur)
    {
        Console.WriteLine($"CONVERSION: valeur * (uniteValeur.Valeur / principale.Valeur) {valeur} * {uniteValeur.Valeur} / {principale.Valeur}");
        return valeur * (uniteValeur.Valeur / principale.Valeur);
    }

    public static double ConvertirPrixUnitaire(Uniteequivalence principale, double valeur, Uniteequivalence uniteValeur)
    {
        return valeur * (principale.Valeur / uniteValeur.Valeur);
    }
}