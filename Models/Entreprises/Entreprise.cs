namespace SIProject.Models.Entreprises;

using SIProject.Models.Cnx;
using SIProject.Models.Shared;
using Npgsql;
using SIProject.Models.Entreprises.Commerce;

[Serializable]
public class Entreprise
{
    private string _id = ""; public string Id{ get{ return _id; } set{ _id = value; } }
    private string _nom = ""; public string Nom { get { return _nom; } set { _nom = value; } }
    private string _email; public string Email { get { return _email != null ? _email : ""; } set { _email = value; } }
    private string _objet = ""; public string Objet { get { return _objet != null ? _objet : ""; } set { _objet = value; } }
    private string _siege = ""; public string Siege { get { return _siege != null ? _siege : ""; } set { _siege = value; } }
    private string _nomChef = ""; public string NomChef { get { return _nomChef != null ? _nomChef : ""; } set { _nomChef = value; } }
    private string _nif = ""; public string Nif { get { return _nif != null ? _nif : ""; } set { _nif = value; } }
    private string _numStat = ""; public string NumStat { get { return _numStat != null ? _numStat : ""; } set { _numStat = value; } }
    private string _status = ""; public string Status { get { return _status != null ? _status : ""; } set { _status = value; } }
    private DateTime _dateCreation; public DateTime DateCreation { get { return _dateCreation; } set { _dateCreation = value; } }
    private string _logoFormat = ""; public string LogoFormat { get { return _logoFormat != null ? _logoFormat : ""; } set { _logoFormat = value; } }
    private string _logoName = ""; public string LogoName { get { return _logoName != null ? _logoName : ""; } set { _logoName = value; } }

    public Entreprise() { }
    public Entreprise(string id, string nom,string email,string objet,string siege,string nomChef,
        string nif,string numStat,string status, DateTime dateCreation, string logoFormat, string logoName)
    {
        Id = id;
        Nom = nom;
        Email = email;
        Objet = objet;
        Siege = siege;
        NomChef = nomChef;
        Nif = nif;
        NumStat = numStat;
        Status = status;
        DateCreation = dateCreation;
        LogoFormat = logoFormat;
        LogoName = logoName;
    }
    public Entreprise(string nom, string email, string objet, string siege, string nomChef,
        string nif, string numStat, string status, DateTime dateCreation, string logoFormat, string logoName)
    {
        Nom = nom;
        Email = email;
        Objet = objet;
        Siege = siege;
        NomChef = nomChef;
        Nif = nif;
        NumStat = numStat;
        Status = status;
        DateCreation = dateCreation;
        LogoFormat = logoFormat;
        LogoName = logoName;
    }

    public Entreprise? Login(NpgsqlConnection cnx,string id, string mdp)
    {
        if (mdp == null)
        {
            throw new Exception("Veuillez entrez votre mot de passe");
        }
        Entreprise? ese = null;
        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }
        string sql = "SELECT * FROM entreprise WHERE id=(SELECT idEse " +
            "FROM Authentification WHERE idEse=@idEse AND mdp=@mdp)";
        using(NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idEse", id);
            command.Parameters.AddWithValue("@mdp", Global.getMd5Hash(mdp));
            Console.WriteLine("Mdp md5 >>> "+ Global.getMd5Hash(mdp));
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ese = new Entreprise(Convert.ToString(reader["id"]),Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["email"]), Convert.ToString(reader["objet"]),
                            Convert.ToString(reader["siege"]), Convert.ToString(reader["nomchef"]),
                            Convert.ToString(reader["nif"]), Convert.ToString(reader["numstat"]),
                            Convert.ToString(reader["status"]), Convert.ToDateTime(reader["dateCreation"]), Convert.ToString(reader["logoformat"]), Convert.ToString(reader["logoName"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER LOGIN ENTREPRISE HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return ese;
    } 

    public int createEntreprise(NpgsqlConnection cnx, Entreprise ese, string id)
    {
        if (ese == null)
        {
            throw new Exception("Entreprise passé en parametre de createEntreprise est null");
        }
        int affectedRow = 0;
        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        String sql = "INSERT INTO entreprise(id,nom,email,objet,siege,nomchef,nif,numstat,status,datecreation,logoformat,logoname) " +
            "VALUES(@id,@nom,@email,@objet,@siege,@nomchef,@nif,@numstat,@status,@datecreation,@logoformat,@logoname)";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nom",ese.Nom);
            command.Parameters.AddWithValue("@email",ese.Email);
            command.Parameters.AddWithValue("@objet",ese.Objet);
            command.Parameters.AddWithValue("@siege",ese.Siege);
            command.Parameters.AddWithValue("@nomchef",ese.NomChef);
            command.Parameters.AddWithValue("@nif",ese.Nif);
            command.Parameters.AddWithValue("@numstat",ese.NumStat);
            command.Parameters.AddWithValue("@status",ese.Status);
            command.Parameters.AddWithValue("@datecreation",ese.DateCreation);
            command.Parameters.AddWithValue("@logoformat", ese.LogoFormat);
            command.Parameters.AddWithValue("@logoname", ese.LogoName);
            
            affectedRow = command.ExecuteNonQuery();
        }

        if (closed)
        {
            cnx.Open();
        }
        return affectedRow;
    }

    public Entreprise? getEntreprise(NpgsqlConnection cnx, string id)
    {
        Entreprise? entreprise = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM Entreprise WHERE id=@id";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@id", id);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        entreprise = new Entreprise(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["email"]), Convert.ToString(reader["objet"]),
                            Convert.ToString(reader["siege"]), Convert.ToString(reader["nomchef"]),
                            Convert.ToString(reader["nif"]), Convert.ToString(reader["numstat"]),
                            Convert.ToString(reader["status"]), Convert.ToDateTime(reader["dateCreation"]), Convert.ToString(reader["logoformat"]), 
                            Convert.ToString(reader["logoName"]));
                    }
                }
                else
                {
                    Console.WriteLine("READER GET ENTREPRISE HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Open();
        }
        return entreprise;
    }

    public List<Entreprise> getListeEntreprises(NpgsqlConnection cnx)
    {
        List<Entreprise> entreprises = new List<Entreprise>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM Entreprise";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Entreprise ese = null;
                    while (reader.Read())
                    {
                        if (!Convert.ToString(reader["id"]).Contains("INDEF"))
                        {
                            ese = new Entreprise(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["email"]), Convert.ToString(reader["objet"]),
                            Convert.ToString(reader["siege"]), Convert.ToString(reader["nomchef"]),
                            Convert.ToString(reader["nif"]), Convert.ToString(reader["numstat"]),
                            Convert.ToString(reader["status"]), Convert.ToDateTime(reader["dateCreation"]), Convert.ToString(reader["logoformat"]), Convert.ToString(reader["logoName"]));

                            entreprises.Add(ese);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("READER GET LISTES ENTREPRISE HAS 0 ROWS");
                }
            }
        }

        if (closed)
        {
            cnx.Open();
        }
        return entreprises;
    }

    public List<Dept> getListeDepartement(NpgsqlConnection cnx)
    {
        List<Dept> depts = new List<Dept>();
        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM dept WHERE idese=@idese";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idese", this.Id);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Dept dept = null;
                    while (reader.Read())
                    {
                        dept = new Dept(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToString(reader["code"]), Convert.ToDateTime(reader["datecreation"]),
                            Convert.ToString(reader["idese"]), Convert.ToString(reader["idresponsable"]));
                        depts.Add(dept);
                    }
                }
                else
                {
                    Console.WriteLine("READER GET LISTES DEPT HAS 0 ROWS");
                }
            }
        }
        if (closed)
        {
            cnx.Open();
        }
        return depts;
    }

    /* ------- ETAT DE STOCK ------- */

    public List<Article> getArticles(NpgsqlConnection cnx, int typeAchat)
    {
        List<Article> articles = new List<Article>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM article WHERE idese=@idese AND typeachat=@type";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idese", this.Id);
            command.Parameters.AddWithValue("@type", 5);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Article? article = null;
                    while (reader.Read())
                    {
                        article = new Article(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToInt32(reader["typeachat"]), Convert.ToString(reader["idese"]));

                        articles.Add(article);
                    }
                }
                else
                {
                    Console.WriteLine("READER GET ARTICLES HAS 0 ROWS");
                }
            }
        }
        if (closed)
        {
            cnx.Close();
        }
        return articles;
    }

    public double getQuantiteInitiale(NpgsqlConnection cnx, string idarticle, DateTime datedebut, DateTime datefin)
    {
        double quantite = 0;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT SUM(entrer) AS quantite FROM mvtdetaillee WHERE idese=@idese AND idarticle=@idarticle" +
            " AND dates>=@datedebut AND dates<=@datefin AND entrer>0 AND etatmvt=15";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idese", this.Id);
            command.Parameters.AddWithValue("@idarticle", idarticle);
            command.Parameters.AddWithValue("@datedebut", datedebut);
            command.Parameters.AddWithValue("@datefin", datefin);

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["quantite"] != DBNull.Value)
                        {
                            quantite = Convert.ToDouble(reader["quantite"]);
                        }
                    }
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return quantite;
    }

    public double getSortie(NpgsqlConnection cnx, string idarticle, DateTime datedebut, DateTime datefin)
    {
        double sortie = 0;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT SUM(sortie) AS sortie FROM mvtdetaillee WHERE idese=@idese AND idarticle=@idarticle" +
            " AND dates>=@datedebut AND dates<=@datefin AND sortie>0 AND etatmvt=15";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idese", this.Id);
            command.Parameters.AddWithValue("@idarticle", idarticle);
            command.Parameters.AddWithValue("@datedebut", datedebut);
            command.Parameters.AddWithValue("@datefin", datefin);

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["sortie"] != DBNull.Value)
                        {
                            sortie = Convert.ToDouble(reader["sortie"]);
                        }
                    }
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }
        return sortie;
    }

    public double getReste(NpgsqlConnection cnx, string idarticle, DateTime datedebut, DateTime datefin)
    {
        double reste = 0;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        reste = this.getQuantiteInitiale(cnx, idarticle, datedebut, datefin) - this.getSortie(cnx, idarticle, 
            datedebut, datefin);

        if (closed)
        {
            cnx.Close();
        }
        return reste;
    }

    public double getPuMoyenne(NpgsqlConnection cnx, string idarticle, DateTime datedebut, DateTime datefin)
    {
        double totalPu = 0;
        double nombre = 0;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sqlNombre = "SELECT COUNT(*) AS nombre FROM mvtdetaillee WHERE idese=@idese AND idarticle=@idarticle" +
            " AND dates>=@datedebut AND dates<=@datefin AND prixunitaire>0 AND entrer>0 AND etatmvt=15";
        using (NpgsqlCommand command = new NpgsqlCommand(sqlNombre, cnx))
        {
            command.Parameters.AddWithValue("@idese", this.Id);
            command.Parameters.AddWithValue("@idarticle", idarticle);
            command.Parameters.AddWithValue("@datedebut", datedebut);
            command.Parameters.AddWithValue("@datefin", datefin);

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["nombre"] != DBNull.Value)
                    {
                        nombre = Convert.ToDouble(reader["nombre"]);
                    }
                }
            }
        }
        string sql = "SELECT SUM(prixunitaire) AS totalpu FROM mvtdetaillee WHERE idese=@idese AND idarticle=@idarticle" +
            " AND dates>=@datedebut AND dates<=@datefin AND prixunitaire>0 AND entrer>0 AND etatmvt=15";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idese", this.Id);
            command.Parameters.AddWithValue("@idarticle", idarticle);
            command.Parameters.AddWithValue("@datedebut", datedebut);
            command.Parameters.AddWithValue("@datefin", datefin);

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["totalpu"] != DBNull.Value)
                    {
                        totalPu = Convert.ToDouble(reader["totalpu"]);
                    }
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }

        double puMoyenne = 0;
        if (nombre != 0)
        {
            puMoyenne = totalPu / nombre;
        }
        return puMoyenne;
    }

    public double getValeurReste(NpgsqlConnection cnx, string idarticle, DateTime datedebut, DateTime datefin)
    {
        double valeur = 0;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        valeur = this.getReste(cnx, idarticle, datedebut, datefin) * this.getPuMoyenne(cnx, idarticle, datedebut, datefin);

        if (closed)
        {
            cnx.Close();
        }

        return valeur;
    }

    public List<EtatDeStock> getEtatsDeStocks(NpgsqlConnection cnx, DateTime datedebut, DateTime datefin)
    {
        List<EtatDeStock> etatDeStocks = new List<EtatDeStock>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        List<Article> articles = this.getArticles(cnx, 5);
        double qi = 0;
        double reste = 0;
        double valeur = 0;
        double pum = 0;
        EtatDeStock eds = null;
        foreach (Article article in articles)
        {
            UniteDetaillee ue = (UniteDetaillee)article.getUnitePrincipale(cnx);
            qi = this.getQuantiteInitiale(cnx, article.Id, datedebut, datefin);
            reste = this.getReste(cnx, article.Id, datedebut, datefin);
            valeur = this.getValeurReste(cnx, article.Id, datedebut, datefin);
            pum = this.getPuMoyenne(cnx, article.Id, datedebut, datefin);

            eds = new EtatDeStock(article, qi, reste, ue, valeur, pum);
            etatDeStocks.Add(eds);
        }
        
        if (closed)
        {
            cnx.Close();
        }
        return etatDeStocks;
    }
}
