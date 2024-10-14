using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce; 
 
 
public class Mouvement {
	string id; 
	string idarticle; 
	double entrer; 
	double sortie; 
	double prixunitaire; 
	DateTime dates; 
	int etatmvt; 
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
 
    public double Entrer{ 
        get{ 
            return entrer; 
        } 
        set{ 
            entrer = value; 
        } 
    } 
 
    public double Sortie{ 
        get{ 
            return sortie; 
        } 
        set{ 
            sortie = value; 
        } 
    } 
 
    public double Prixunitaire{ 
        get{ 
            return prixunitaire; 
        } 
        set{ 
            prixunitaire = value; 
        } 
    } 
 
    public DateTime Dates{ 
        get{ 
            return dates; 
        } 
        set{ 
            dates = value; 
        } 
    } 
 
    public int Etatmvt{ 
        get{ 
            return etatmvt; 
        } 
        set{ 
            etatmvt = value; 
        } 
    }

    public Mouvement() { }

    public Mouvement(string id, string idarticle, double entrer, double sortie, double prixunitaire, DateTime dates, int etatmvt)
    {
        Id = id;
        Idarticle = idarticle;
        Entrer = entrer;
        Sortie = sortie;
        Prixunitaire = prixunitaire;
        Dates = dates;
        Etatmvt = etatmvt;
    }

    public Mouvement(string idarticle, double entrer, double sortie, double prixunitaire, DateTime dates, int etatmvt)
    {
        Idarticle = idarticle;
        Entrer = entrer;
        Sortie = sortie;
        Prixunitaire = prixunitaire;
        Dates = dates;
        Etatmvt = etatmvt;
    }

    /* -------- CRUD --------- */

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

        string sql = "INSERT INTO Mouvement(id,idarticle,entrer,sortie,prixunitaire,dates,etatmvt) " +
            "VALUES(@id,@idarticle,@entrer,@sortie,@prixunitaire,@dates,@etatmvt)";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            string newid = "";
            if (this.Id == null)
            {
                newid = Connex.createId(cnx, "mouvement_sequence", "MVT", 7);
            }
            else
            {
                newid = this.Id;
            }
            command.Parameters.AddWithValue("@id", newid);
            command.Parameters.AddWithValue("@idarticle", this.Idarticle);
            command.Parameters.AddWithValue("@entrer", this.Entrer);
            command.Parameters.AddWithValue("@sortie", this.Sortie);
            command.Parameters.AddWithValue("@prixunitaire", this.Prixunitaire);
            command.Parameters.AddWithValue("@dates", this.Dates);
            command.Parameters.AddWithValue("@etatmvt", this.Etatmvt);

            affectedRow = command.ExecuteNonQuery();
        }
        if (closed)
        {
            cnx.Close();
        }
        return affectedRow;
    }

    public int update(NpgsqlConnection cnx)
    {
        int affectedRow = 0;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "UPDATE Mouvement SET entrer=@entrer,sortie=@sortie,prixunitaire=@pu,dates=@dates,etatmvt=@etat " +
            "WHERE id=@id";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@entrer", this.Entrer);
            command.Parameters.AddWithValue("@sortie", this.Sortie);
            command.Parameters.AddWithValue("@pu", this.Prixunitaire);
            command.Parameters.AddWithValue("@dates", this.Dates);
            command.Parameters.AddWithValue("@etat", this.Etatmvt);
            command.Parameters.AddWithValue("@id", this.Id);

            affectedRow = command.ExecuteNonQuery();
        }
        if (closed)
        {
            cnx.Close();
        }
        return affectedRow;
    }

    public static Mouvement? getMouvement(NpgsqlConnection cnx, string idmvt)
    {
        Console.WriteLine("IDMVT DANS GETMouvement: "+idmvt);
        Mouvement? mouvement = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM mouvement WHERE id=@idmvt";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idmvt", idmvt);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mouvement = new Mouvement(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToDouble(reader["entrer"]), Convert.ToDouble(reader["sortie"]), 
                            Convert.ToDouble(reader["prixunitaire"]),Convert.ToDateTime(reader["dates"]), Convert.ToInt32(reader["etatmvt"]));
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
        return mouvement;
    }

    public static Mouvement? getMouvementByEtat(NpgsqlConnection cnx, int etat)
    {
        Mouvement? mouvement = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM mouvement WHERE etatmvt=@etat";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@etat", etat);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mouvement = new Mouvement(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToDouble(reader["entrer"]), Convert.ToDouble(reader["sortie"]),
                            Convert.ToDouble(reader["prixunitaire"]), Convert.ToDateTime(reader["dates"]), Convert.ToInt32(reader["etatmvt"]));
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
        return mouvement;
    }

    public static Mouvement? getMouvementByEtatAndType(NpgsqlConnection cnx, int etat, int type)
    {
        Mouvement? mouvement = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM mvtdetaillee WHERE etatmvt=@etat and typeachat=@type";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@etat", etat);
            command.Parameters.AddWithValue("@type", type);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mouvement = new Mouvement(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToDouble(reader["entrer"]), Convert.ToDouble(reader["sortie"]),
                            Convert.ToDouble(reader["prixunitaire"]), Convert.ToDateTime(reader["dates"]), Convert.ToInt32(reader["etatmvt"]));
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
        return mouvement;
    }

    public static List<MvtDetaillee> getAchatEnAttente(NpgsqlConnection cnx)
    {
        List<MvtDetaillee> achats = new List<MvtDetaillee>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM mvtdetaillee WHERE etatmvt=1 and entrer>sortie ORDER BY dates ASC";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    MvtDetaillee achat = null;
                    while (reader.Read())
                    {
                        achat = new MvtDetaillee(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToString(reader["nomarticle"]), Convert.ToInt32(reader["typeachat"]),
                            Convert.ToString(reader["idese"]), Convert.ToDouble(reader["entrer"]), 
                            Convert.ToDouble(reader["sortie"]), Convert.ToDouble(reader["prixunitaire"]), 
                            Convert.ToDateTime(reader["dates"]), Convert.ToInt32(reader["etatmvt"]));
                        if (achat.Typeachat == 1)
                        {
                            achat.Background = "lightgrey";
                        }
                        else if(achat.Typeachat == 5)
                        {
                            achat.Background = "lightgreen";
                        }
                        achats.Add(achat);
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
        return achats;
    }

    /* --- Proforma --- */
    public int addProforma(NpgsqlConnection cnx, Proforma proforma)
    {
        int affectedRow = 0;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        using(NpgsqlTransaction transaction = cnx.BeginTransaction())
        {
            try
            {
                affectedRow += proforma.save(cnx);
                string sql = "INSERT INTO proformamvt(id,idmvt,idproforma) VALUES(default,@idmvt,@idproforma)";
                using (NpgsqlCommand command = new NpgsqlCommand(sql,cnx))
                {
                    command.Parameters.AddWithValue("@idmvt", this.Id);
                    command.Parameters.AddWithValue("@idproforma", proforma.Id);

                    affectedRow += command.ExecuteNonQuery();
                }
                transaction.Commit();
            }catch(Exception e)
            {
                transaction.Rollback();
                Console.Error.WriteLine("ERROR: \n" + e);
                throw;
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return affectedRow;
    }

    public List<String> getIdProformas(NpgsqlConnection cnx)
    {
        List<String> idproformas = new List<String>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT idproforma FROM proformamvt WHERE idmvt=@idmvt";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idmvt", this.Id);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idproformas.Add(Convert.ToString(reader["idproforma"]));
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
        return idproformas;
    }

    public List<Proforma> getProformas(NpgsqlConnection cnx)
    {
        List<Proforma> proformas = new List<Proforma>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT idproforma FROM proformamvt WHERE idmvt=@idmvt";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idmvt", this.Id);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Proforma proforma = null;
                    while (reader.Read())
                    {
                        proformas.Add(Proforma.getProforma(cnx, Convert.ToString(reader["idproforma"])));
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
        return proformas;
    }

    /* -------- FIN CRUD --------- */
}