using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class BonDeCommande
    {
        private string _id = "";
        private string _idProforma = "";
        private double _prixHt;
        private double _tva;
        private double _prixTtc;
        private DateTime _dateCommande;
        private DateTime _dateLivraison;
        private int _statut;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string IdProforma
        {
            get { return _idProforma; }
            set { _idProforma = value; }
        }
        public double PrixHt
        {
            get { return _prixHt; }
            set { _prixHt = value; }
        }
        public double Tva
        {
            get { return _tva; }
            set { _tva = value; }
        }
        public double PrixTtc
        {
            get { return _prixTtc; }
            set { _prixTtc = value; }
        }
        public DateTime DateCommande
        {
            get { return _dateCommande; }
            set { _dateCommande = value; }
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

        public BonDeCommande()
        {
        }

        public BonDeCommande(string id, string idProforma, double prixHt, double tva, double prixTtc, 
            DateTime dateCommande, DateTime dateLivraison, int statut)
        {
            Id = id;
            IdProforma = idProforma;
            PrixHt = prixHt;
            Tva = tva;
            PrixTtc = prixTtc;
            DateCommande = dateCommande;
            DateLivraison = dateLivraison;
            Statut = statut;
        }
        public BonDeCommande(string idProforma, double prixHt, double tva, double prixTtc, 
            DateTime dateCommande, DateTime dateLivraison, int statut)
        {
            IdProforma = idProforma;
            PrixHt = prixHt;
            Tva = tva;
            PrixTtc = prixTtc;
            DateCommande = dateCommande;
            DateLivraison = dateLivraison;
            Statut = statut;
        }

        public int Save(NpgsqlConnection cnx)
        {
            int insertedRow = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }
            string newId = this.Id;
            if (String.IsNullOrWhiteSpace(newId))
            {
                newId = Connex.createId(cnx, "bondecommande_sequence", "BCI", 7);
            }

            string sql = "INSERT INTO BonDeCommande(id,idproforma,prixht,tva,prixttc,datecommande,delailivraison" +
                ",statut) VALUES(@id,@idproforma,@prixht,@tva,@prixttc,@datecommande,@delailivraison,@statut)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", newId);
                command.Parameters.AddWithValue("@idproforma", this.IdProforma);
                command.Parameters.AddWithValue("@prixht", this.PrixHt);
                command.Parameters.AddWithValue("@tva", this.Tva);
                command.Parameters.AddWithValue("@prixttc", this.PrixTtc);
                command.Parameters.AddWithValue("@datecommande", this.DateCommande);
                command.Parameters.AddWithValue("@delailivraison", this.DateLivraison);
                command.Parameters.AddWithValue("@statut", this.Statut);

                insertedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return insertedRow;
        }

        public static int UpdateStatut(NpgsqlConnection cnx, string idBonDeCommande, int statut)
        {
            int affectedRow = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "UPDATE BonDeCommande SET statut = @newstatut WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@newstatut", statut);
                command.Parameters.AddWithValue("@id", idBonDeCommande);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static List<BonDeCommande> GetBonDeCommandes(NpgsqlConnection cnx, string idCatImmo, int statut)
        {
            List<BonDeCommande> bonDeCommandes = new List<BonDeCommande>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_bondecommande WHERE idCatImmo = @idcatimmo AND statut = @statut";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idcatimmo", idCatImmo);
                command.Parameters.AddWithValue("@statut", statut);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        BonDeCommande? bonDeCommande = null;
                        while (reader.Read())
                        {
                            string id = reader.GetString(0);
                            string idproforma = reader.GetString(3);
                            double prixht = reader.GetDouble(4);
                            double tva = reader.GetDouble(5);
                            double prixttc = reader.GetDouble(6);
                            DateTime datecommande = reader.GetDateTime(7);
                            DateTime datelivraison = reader.GetDateTime(8);

                            bonDeCommande = new BonDeCommande(id, idproforma, prixht, tva, prixttc, datecommande, datelivraison, statut);
                            bonDeCommandes.Add(bonDeCommande);
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
            return bonDeCommandes;
        }

        public static BonDeCommande? GetBonDeCommande(NpgsqlConnection cnx, string id)
        {
            BonDeCommande? bonDeCommande = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM bondecommande WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string idproforma = reader.GetString(1);
                            double prixht = reader.GetDouble(2);
                            double tva = reader.GetDouble(3);
                            double prixttc = reader.GetDouble(4);
                            DateTime datecommande = reader.GetDateTime(5);
                            DateTime datelivraison = reader.GetDateTime(6);
                            int statut = reader.GetInt32(7);

                            bonDeCommande = new BonDeCommande(id, idproforma, prixht, tva, prixttc, datecommande, datelivraison, statut);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET BON DE COMMANDE BY ID HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return bonDeCommande;
        }
    }
}
