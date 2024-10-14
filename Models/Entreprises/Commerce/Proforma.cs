using MailKit.Net.Smtp;
using MimeKit;
using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises.User.EmailService;

namespace SIProject.Models.Entreprises.Commerce; 
 
 
public class Proforma {
	string id; 
	string idarticle; 
	double quantite; 
	double prixunitaire;
    string iduniteequivalence;
    DateTime dateemission; 
	DateTime dateexpiration; 
	int etat; 
	string idfournisseur; 
	string idclient; 
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
 
    public double Quantite{ 
        get{ 
            return quantite; 
        } 
        set{ 
            quantite = value; 
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

    public string Iduniteequivalence
    {
        get
        {
            return iduniteequivalence;
        }
        set
        {
            iduniteequivalence = value;
        }
    }

    public DateTime Dateemission{ 
        get{ 
            return dateemission; 
        } 
        set{ 
            dateemission = value; 
        } 
    } 
 
    public DateTime Dateexpiration{ 
        get{ 
            return dateexpiration; 
        } 
        set{ 
            dateexpiration = value; 
        } 
    } 
 
    public int Etat{ 
        get{ 
            return etat; 
        } 
        set{ 
            etat = value; 
        } 
    } 
 
    public string Idfournisseur{ 
        get{ 
            return idfournisseur; 
        } 
        set{ 
            idfournisseur = value; 
        } 
    } 
 
    public string Idclient{ 
        get{ 
            return idclient; 
        } 
        set{ 
            idclient = value; 
        } 
    } 

    public Proforma() { }

    public Proforma(string id,string idarticle,double quantite,double prixunitaire, string iduniteequivalence, DateTime dateemission,
        DateTime dateexpiration,int etat,string idfournisseur,string idclient)
    {
        Id = id;
        Idarticle = idarticle;
        Quantite = quantite;
        Prixunitaire = prixunitaire;
        Iduniteequivalence = iduniteequivalence;
        Dateemission = dateemission;
        Dateexpiration = dateexpiration;
        Etat = etat;
        Idfournisseur = idfournisseur;
        Idclient = idclient;
    }

    public Proforma(string idarticle, double quantite, double prixunitaire, string iduniteequivalence, DateTime dateemission,
        DateTime dateexpiration, int etat, string idfournisseur, string idclient)
    {
        Idarticle = idarticle;
        Quantite = quantite;
        Prixunitaire = prixunitaire;
        Iduniteequivalence = iduniteequivalence;
        Dateemission = dateemission;
        Dateexpiration = dateexpiration;
        Etat = etat;
        Idfournisseur = idfournisseur;
        Idclient = idclient;
    }

    /* ----------- DEBUT CRUD ------------ */

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

        String sql = "INSERT INTO Proforma(id,idarticle,quantite,prixunitaire,iduniteequivalence,dateemission," +
            "dateexpiration,etat,idfournisseur,idclient) VALUES(@id,@idarticle,@quantite,@prixunitaire," +
            "@iduniteequivalence,@dateemission,@dateexpiration,@etat,@idfournisseur,@idclient)";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            string newid = "";
            if (this.Id == null)
            {
                newid = Connex.createId(cnx, "proforma_sequence", "PFM", 7);
            }
            else
            {
                newid = this.Id;
            }
            command.Parameters.AddWithValue("@id", newid);
            command.Parameters.AddWithValue("@idarticle", this.Idarticle);
            command.Parameters.AddWithValue("@quantite", this.Quantite);
            command.Parameters.AddWithValue("@prixunitaire", this.Prixunitaire);
            command.Parameters.AddWithValue("@iduniteequivalence", this.Iduniteequivalence);
            command.Parameters.AddWithValue("@dateemission", this.Dateemission);
            command.Parameters.AddWithValue("@dateexpiration", this.Dateexpiration);
            command.Parameters.AddWithValue("@etat", this.Etat);
            command.Parameters.AddWithValue("@idfournisseur", this.Idfournisseur);
            command.Parameters.AddWithValue("@idclient", this.Idclient);

            affectedRow = command.ExecuteNonQuery();
        }

        if (closed)
        {
            cnx.Close();
        }
        return affectedRow;
    }

    public static Proforma? getProforma(NpgsqlConnection cnx, string idproforma)
    {
        Proforma? proforma = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM proforma WHERE id=@idprof";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idprof", idproforma);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        proforma = new Proforma(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToDouble(reader["quantite"]), Convert.ToDouble(reader["prixunitaire"]),
                            Convert.ToString(reader["iduniteequivalence"]), Convert.ToDateTime(reader["dateemission"]),
                            Convert.ToDateTime(reader["dateexpiration"]), Convert.ToInt32(reader["etat"]),
                            Convert.ToString(reader["idfournisseur"]), Convert.ToString(reader["idclient"]));
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
        return proforma;
    }

    public List<Proforma> getProformas(NpgsqlConnection cnx, string idEse)
    {
        List<Proforma> proformas = new List<Proforma>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM proformaese WHERE idese=@idese";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idese", idEse);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Proforma? proforma = null;
                    while (reader.Read())
                    {
                        proforma = new Proforma(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToDouble(reader["quantite"]), Convert.ToDouble(reader["prixunitaire"]),
                            Convert.ToString(reader["iduniteequivalence"]), Convert.ToDateTime(reader["dateemission"]),
                            Convert.ToDateTime(reader["dateexpiration"]), Convert.ToInt32(reader["etat"]),
                            Convert.ToString(reader["idfournisseur"]), Convert.ToString(reader["idclient"]));

                        proformas.Add(proforma);
                    }
                }
                else
                {
                    Console.WriteLine("READER GET LIST PROFORMAS HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return proformas;
    }

    /* ------------- FIN CRUD -------------- */

    public async void sendMail(NpgsqlConnection cnx, IEmailSender emailSender, string sender, string senderMail,
        string receiver, string receiverMail)
    {
        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        Article article = Article.getArticle(cnx, this.Idarticle) ?? throw new Exception($"Cet Article de l'id: {this.idarticle} n'existe pas");
        UniteDetaillee unite = (UniteDetaillee) UniteDetaillee.getUnite(cnx, this.Iduniteequivalence) ?? throw new Exception($"L'unite d'equivalence par l'id: {this.Iduniteequivalence} n'existe pas");

        string subject = "Proforma: " + article.Nom;
        string contenu = $"Quantité en stock: {this.Quantite} {unite.Unite}, Prix unitaire: {this.Prixunitaire}Ar/{unite.Unite}, " +
            $"Date d'émission: {this.Dateemission}, Date d'éxpiration de ce Proforma: {this.Dateexpiration}";
        var message = new Message(new string[] { receiverMail }, subject, contenu);

        await emailSender.SendEmailAsync(sender, message);

        if (closed)
        {
            cnx.Close();
        }
    }

    public static void receiveMail(IEmailSender emailSender)
    {
        emailSender.ReceiveEmails();
    }
}