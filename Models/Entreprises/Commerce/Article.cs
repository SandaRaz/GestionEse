using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce; 
 
 
public class Article {
	string id; 
	string nom; 
	int typeachat;
    string idese;
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
 
    public int Typeachat{ 
        get{ 
            return typeachat; 
        } 
        set{ 
            typeachat = value; 
        } 
    }

    public string Idese
    {
        get
        {
            return idese;
        }
        set
        {
            idese = value;
        }
    }

    public Article(string id,string nom,int typeachat,string idese)
    {
        Id = id;
        Nom = nom;
        Typeachat = typeachat;
        Idese = idese;
    }
    public Article(string nom, int typeachat, string idese)
    {
        Nom = nom;
        Typeachat = typeachat;
        Idese = idese;
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

        String sql = "INSERT INTO article(id,nom,typeachat,idese) " +
            "VALUES(@id,@nom,@typeachat,@idese)";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            string newid = "";
            if (this.Id == null)
            {
                newid = Connex.createId(cnx, "article_sequence", "ART", 7);
            }
            else
            {
                newid = this.Id;
            }
            command.Parameters.AddWithValue("id", newid);
            command.Parameters.AddWithValue("@nom", this.Nom);
            command.Parameters.AddWithValue("@typeachat", this.Typeachat);
            command.Parameters.AddWithValue("@idese", this.Idese);

            affectedRow = command.ExecuteNonQuery();
        }

        if (closed)
        {
            cnx.Close();
        }
        return affectedRow;
    }

    public static Article? getArticle(NpgsqlConnection cnx, string idarticle)
    {
        Article? article = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM article WHERE id=@idarticle";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idarticle", idarticle);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        article = new Article(Convert.ToString(reader["id"]), Convert.ToString(reader["nom"]),
                            Convert.ToInt32(reader["typeachat"]), Convert.ToString(reader["idese"]));
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
        return article;
    }

    public static List<Article> getArticles(NpgsqlConnection cnx)
    {
        List<Article> articles = new List<Article>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM article";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
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

    public static List<Article> getArticles(NpgsqlConnection cnx,string idese)
    {
        List<Article> articles = new List<Article>();

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM article WHERE idese=@idese ";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idese", idese);
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

    public static List<Article> getArticles(NpgsqlConnection cnx, string idese, int typeAchat)
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
            command.Parameters.AddWithValue("@idese", idese);
            command.Parameters.AddWithValue("@type", typeAchat);
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

    public Uniteequivalence? getUnitePrincipale(NpgsqlConnection cnx)
    {
        Uniteequivalence? unite = null;

        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        string sql = "SELECT * FROM unitedetaillee WHERE idarticle=@idarticle AND valeur=1";

        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idarticle", this.Id);

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["id"] != DBNull.Value)
                        {
                            unite = new UniteDetaillee(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToString(reader["article"]), Convert.ToString(reader["idunite"]), Convert.ToString(reader["unite"]), Convert.ToDouble(reader["valeur"]));
                        }
                    }
                }
            }
        }

        if (closed)
        {
            cnx.Close();
        }

        if (unite == null)
        {
            throw new Exception("CET ARTICLE N'A PAS UNITE PRINCIPALE");
        }

        return unite;
    }

    public UniteDetaillee[]? getUnitesEquivalences(NpgsqlConnection cnx)
    {
        bool closed = false;
        if (cnx.State == System.Data.ConnectionState.Closed)
        {
            cnx = Connex.getConnection();
            cnx.Open();
            closed = true;
        }

        UniteDetaillee[]? unites = null;

        string sql = "SELECT * FROM unitedetaillee WHERE idarticle=@idarticle";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
        {
            command.Parameters.AddWithValue("@idarticle", this.Id);
            int nbLigne = 0;

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["id"] != DBNull.Value)
                        {
                            nbLigne++;
                        }
                    }
                }
            }

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (nbLigne > 0)
                {
                    unites = new UniteDetaillee[nbLigne];
                    int ligne = 0;
                    while (reader.Read())
                    {
                        unites[ligne] = new UniteDetaillee(Convert.ToString(reader["id"]), Convert.ToString(reader["idarticle"]),
                            Convert.ToString(reader["article"]), Convert.ToString(reader["idunite"]), Convert.ToString(reader["unite"]), Convert.ToDouble(reader["valeur"]));
                        ligne++;
                    }
                }

            }
        }

        if (closed)
        {
            cnx.Close();
        }
        if (unites == null)
        {
            throw new Exception("CET ARTICLE N'A AUCUN UNITE D'EQUIVALENCE");
        }

        return unites;
    }
}