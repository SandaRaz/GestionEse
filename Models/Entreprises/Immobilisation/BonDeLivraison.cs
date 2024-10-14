using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class BonDeLivraison
    {
        private string _id = "";
        private string _idBonDeCommande = "";
        private double _quantiteLivree;
        private double _prix;
        private double _frais;
        private string _descriptions = "";
        private string _observations = "";
        private string _adresse = "";
        private string _livreur = "";
        private string _contactLivreur = "";
        private DateTime _dateLivraison;
        private int _statut;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string IdBonDeCommande
        {
            get { return _idBonDeCommande; }
            set { _idBonDeCommande = value; }
        }
        public double QuantiteLivree
        {
            get { return _quantiteLivree; }
            set { _quantiteLivree = value; }
        }
        public double Prix
        {
            get { return _prix; }
            set { _prix = value; }
        }
        public double Frais
        {
            get { return _frais; }
            set { _frais = value; }
        }
        public string Descriptions
        {
            get { return _descriptions; }
            set { _descriptions = value; }
        }
        public string Observations
        {
            get { return _observations; }
            set { _observations = value; }
        }
        public string Adresse
        {
            get { return _adresse; }
            set { _adresse = value; }
        }
        public string Livreur
        {
            get { return _livreur; }
            set { _livreur = value; }
        }
        public string ContactLivreur
        {
            get { return _contactLivreur; }
            set { _contactLivreur = value; }
        }
        public DateTime DateLivraison
        {
            get { return _dateLivraison; }
            set { _dateLivraison = value; }
        }
        public int Statut
        {
            get { return _statut; }
            set { _statut = value; }
        }

        public BonDeLivraison()
        {
        }

        public BonDeLivraison(string id, string idBonDeCommande, double quantiteLivree, double prix, 
            double frais, string descriptions, string observations, string adresse, string livreur, 
            string contactLivreur, DateTime dateLivraison, int statut)
        {
            Id = id;
            IdBonDeCommande = idBonDeCommande;
            QuantiteLivree = quantiteLivree;
            Prix = prix;
            Frais = frais;
            Descriptions = descriptions;
            Observations = observations;
            Adresse = adresse;
            Livreur = livreur;
            ContactLivreur = contactLivreur;
            DateLivraison = dateLivraison;
            Statut = statut;
        }
        public BonDeLivraison(string idBonDeCommande, double quantiteLivree, double prix,
            double frais, string descriptions, string observations, string adresse, string livreur,
            string contactLivreur, DateTime dateLivraison, int statut)
        {
            IdBonDeCommande = idBonDeCommande;
            QuantiteLivree = quantiteLivree;
            Prix = prix;
            Frais = frais;
            Descriptions = descriptions;
            Observations = observations;
            Adresse = adresse;
            Livreur = livreur;
            ContactLivreur = contactLivreur;
            DateLivraison = dateLivraison;
            Statut = statut;
        }

        public int Save(NpgsqlConnection cnx)
        {
            int affectedRow = 0;
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string newid = this.Id;
            if (String.IsNullOrWhiteSpace(newid))
            {
                newid = Connex.createId(cnx, "bondecommande_sequence", "BDC", 7);
            }
            String sql = "INSERT INTO BonDeLivraison(id,idbondecommande,quantitelivree,prix,frais," +
                "descriptions,observations,adresse,livreur,contactLivreur,dateLivraison,statut) VALUES(@id," +
                "@idbondecommande,@quantitelivree,@prix,@frais,@descriptions,@observations,@adresse," +
                "@livreur,@contactLivreur,@dateLivraison,@statut)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                if (String.IsNullOrWhiteSpace(this.Descriptions))
                {
                    this.Descriptions = "";
                }
                if (String.IsNullOrWhiteSpace(this.Observations))
                {
                    this.Observations = "";
                }

                command.Parameters.AddWithValue("@id", newid);
                command.Parameters.AddWithValue("@idbondecommande", this.IdBonDeCommande);
                command.Parameters.AddWithValue("@quantitelivree", this.QuantiteLivree);
                command.Parameters.AddWithValue("@prix", this.Prix);
                command.Parameters.AddWithValue("@frais", this.Frais);
                command.Parameters.AddWithValue("@descriptions", this.Descriptions);
                command.Parameters.AddWithValue("@observations", this.Observations);
                command.Parameters.AddWithValue("@adresse", this.Adresse);
                command.Parameters.AddWithValue("@livreur", this.Livreur);
                command.Parameters.AddWithValue("@contactLivreur", this.ContactLivreur);
                command.Parameters.AddWithValue("@dateLivraison", this.DateLivraison);
                command.Parameters.AddWithValue("@statut", this.Statut);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static int UpdateStatut(NpgsqlConnection cnx, string idBonDeLivraison, int statut)
        {
            int affectedRow = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "UPDATE BonDeLivraison SET statut = @newstatut WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@newstatut", statut);
                command.Parameters.AddWithValue("@id", idBonDeLivraison);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static List<BonDeLivraison> GetBonDeLivraisons(NpgsqlConnection cnx, string idCatImmo, int statut)
        {
            List<BonDeLivraison> bonDeLivraisons = new List<BonDeLivraison>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_bondelivraison WHERE idCatImmo = @idcatimmo AND statut = @statut";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idcatimmo", idCatImmo);
                command.Parameters.AddWithValue("@statut", statut);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        BonDeLivraison? bonDeLivraison = null;
                        while (reader.Read())
                        {
                            string id = reader.GetString(0);
                            string idbondecommande = reader.GetString(1);
                            double quantiteLivree = reader.GetDouble(4);
                            double prix = reader.GetDouble(5);
                            double frais = reader.GetDouble(6);
                            string descriptions = reader.GetString(7);
                            string observations = reader.GetString(8);
                            string adresse = reader.GetString(9);
                            string livreur = reader.GetString(10);
                            string contactLivreur = reader.GetString(11);
                            DateTime dateLivraison = reader.GetDateTime(12);

                            bonDeLivraison = new BonDeLivraison(id, idbondecommande, quantiteLivree, prix, frais
                                , descriptions, observations, adresse, livreur, contactLivreur, dateLivraison, statut);

                            bonDeLivraisons.Add(bonDeLivraison);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET BON DE COMMANDES HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return bonDeLivraisons;
        }

        public static BonDeLivraison? GetBonDeLivraison(NpgsqlConnection cnx, string id)
        {
            BonDeLivraison? bonDeLivraison = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM bondelivraison WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string idbondecommande = reader.GetString(1);
                            double quantiteLivree = reader.GetDouble(2);
                            double prix = reader.GetDouble(3);
                            double frais = reader.GetDouble(4);
                            string descriptions = reader.GetString(5);
                            string observations = reader.GetString(6);
                            string adresse = reader.GetString(7);
                            string livreur = reader.GetString(8);
                            string contactLivreur = reader.GetString(9);
                            DateTime dateLivraison = reader.GetDateTime(10);
                            int statut = reader.GetInt32(11);

                            bonDeLivraison = new BonDeLivraison(id, idbondecommande, quantiteLivree, prix, frais
                                , descriptions, observations, adresse, livreur, contactLivreur, dateLivraison, statut);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET BON DE LIVRAISON HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return bonDeLivraison;
        }

        public static string GetIdImmobilier(NpgsqlConnection cnx, string idBonDeLivraison)
        {
            string idImmobilier = "";

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT idImmobilier FROM v_BonDeLivraison WHERE id = @id";
            using(NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", idBonDeLivraison);

                Object res = command.ExecuteScalar() ?? throw new Exception($"CANNOT FIND idImmobilier FROM v_idBonDeLivraison WITH id {idBonDeLivraison}");
                idImmobilier = Convert.ToString(res) ?? throw new Exception("Convert NULL String");
            }

            if (closed)
            {
                cnx.Close();
            }
            return idImmobilier;
        }
    }
}
