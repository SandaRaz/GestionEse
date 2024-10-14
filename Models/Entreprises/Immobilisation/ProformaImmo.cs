using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class ProformaImmo
    {
        private string _id = "";
        private string _idImmobilier = "";
        private double _quantite;
        private double _prix;
        private DateTime _dateEmission;
        private DateTime _dateExpiration;
        private string _idEtat = "";
        private string _idFournisseur = "";
        private string _fournisseur = "";
        private string _reference = "";
        private int _statut;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string IdImmobilier
        {
            get { return _idImmobilier; }
            set { _idImmobilier = value; }
        }
        public double Quantite
        {
            get { return _quantite; }
            set { _quantite = value; }
        }
        public double Prix
        {
            get { return _prix; }
            set { _prix = value; }
        }
        public DateTime DateEmission
        {
            get { return _dateEmission; }
            set { _dateEmission = value; }
        }
        public DateTime DateExpiration
        {
            get { return _dateExpiration; }
            set { _dateExpiration = value; }
        }
        public string IdEtat
        {
            get { return _idEtat; }
            set { _idEtat = value; }
        }
        public string IdFournisseur
        {
            get { return _idFournisseur; }
            set { _idFournisseur = value; }
        }
        public string Fournisseur
        {
            get { return _fournisseur; }
            set { _fournisseur = value; }
        }
        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }
        public int Statut
        {
            get { return _statut; }
            set { _statut = value; }
        }

        public ProformaImmo()
        {
        }

        public ProformaImmo(string id, string idImmobilier, double quantite, double prix, DateTime dateEmission,
            DateTime dateExpiration, string idEtat, string idFournisseur, string fournisseur, string reference, int statut)
        {
            Id = id;
            IdImmobilier = idImmobilier;
            Quantite = quantite;
            Prix = prix;
            DateEmission = dateEmission;
            DateExpiration = dateExpiration;
            IdEtat = idEtat;
            IdFournisseur = idFournisseur;
            Fournisseur = fournisseur;
            Reference = reference;
            Statut = statut;
        }
        public ProformaImmo(string idImmobilier, double quantite, double prix, DateTime dateEmission, 
            DateTime dateExpiration, string idEtat, string idFournisseur, string fournisseur, string reference, int statut)
        {
            IdImmobilier = idImmobilier;
            Quantite = quantite;
            Prix = prix;
            DateEmission = dateEmission;
            DateExpiration = dateExpiration;
            IdEtat = idEtat;
            IdFournisseur = idFournisseur;
            Fournisseur = fournisseur;
            Reference = reference;
            Statut = statut;
        }

        public int save(NpgsqlConnection cnx)
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
                newId = Connex.createId(cnx, "proformaimmo_sequence", "PFI", 7);
            }

            string sql = "INSERT INTO ProformaImmo(id,idImmobilier,quantite,prix,dateemission,dateexpiration," +
                "idetat,idfournisseur,fournisseur,reference,statut) VALUES(@id,@idImmobilier,@quantite,@prix,@dateemission," +
                "@dateexpiration,@idetat,@idfournisseur,@fournisseur,@reference,@statut)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                if(this.Fournisseur == null)
                {
                    this.Fournisseur = "";
                }
                if(this.Reference == null)
                {
                    this.Reference = "";
                }

                command.Parameters.AddWithValue("@id", newId);
                command.Parameters.AddWithValue("@idImmobilier", this.IdImmobilier);
                command.Parameters.AddWithValue("@quantite", this.Quantite);
                command.Parameters.AddWithValue("@prix", this.Prix);
                command.Parameters.AddWithValue("@dateemission", this.DateEmission);
                command.Parameters.AddWithValue("@dateexpiration", this.DateExpiration);
                command.Parameters.AddWithValue("@idetat", this.IdEtat);
                command.Parameters.AddWithValue("@idfournisseur", this.IdFournisseur);
                command.Parameters.AddWithValue("@fournisseur", this.Fournisseur);
                command.Parameters.AddWithValue("@reference", this.Reference);
                command.Parameters.AddWithValue("@statut", this.Statut);

                insertedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return insertedRow;
        }

        public static int UpdateStatut(NpgsqlConnection cnx, string idProforma, int statut)
        {
            int affectedRow = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "UPDATE ProformaImmo SET statut = @newstatut WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@newstatut", statut);
                command.Parameters.AddWithValue("@id", idProforma);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static ProformaImmo? GetProformaImmo(NpgsqlConnection cnx, string id)
        {
            ProformaImmo? proforma = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM ProformaImmo WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string idImmo = reader.GetString(1);
                            double quantite = reader.GetDouble(2);
                            double prix = reader.GetDouble(3);
                            DateTime dateemission = reader.GetDateTime(4);
                            DateTime dateexpiration = reader.GetDateTime(5);
                            string idetat = reader.GetString(6);
                            string idfournisseur = reader.GetString(7);
                            string fournisseur = reader.GetString(8);
                            string reference = reader.GetString(9);
                            int statut = reader.GetInt32(10);

                            proforma = new ProformaImmo(id, idImmo, quantite, prix, dateemission, dateexpiration,
                                idetat, idfournisseur, fournisseur, reference, statut);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET PROFORMAIMMO BY ID HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return proforma;
        }

        public static List<ProformaImmo> GetProformaImmos(NpgsqlConnection cnx, string idCatImmo, int statut)
        {
            List<ProformaImmo> proformas = new List<ProformaImmo>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_ProformaImmo WHERE idCatImmo = @idcatimmo AND statut = @statut";
            using(NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idcatimmo", idCatImmo);
                command.Parameters.AddWithValue("@statut", statut);
                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ProformaImmo? proforma = null;
                        while (reader.Read())
                        {
                            string id = reader.GetString(0);
                            string idImmo = reader.GetString(1);
                            double quantite = reader.GetDouble(3);
                            double prix = reader.GetDouble(4);
                            DateTime dateemission = reader.GetDateTime(5);
                            DateTime dateexpiration = reader.GetDateTime(6);
                            string idetat = reader.GetString(7);
                            string idfournisseur = reader.GetString(8);
                            string fournisseur = reader.GetString(9);
                            string reference = reader.GetString(10);

                            proforma = new ProformaImmo(id, idImmo, quantite, prix, dateemission, dateexpiration, 
                                idetat, idfournisseur, fournisseur, reference, statut);
                            proformas.Add(proforma);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET PROFORMAS IMMO HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return proformas;
        }

        // --------------------------------------------------------------
        public double getTVA()
        {
            return (this.Prix * 20) / 100;
        }
        public double getPrixTTC()
        {
            return (this.getTVA() + this.Prix);
        }
    }
}
