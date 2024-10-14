using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class PvReception
    {
        private string _id = "";
        private string _idImmobilier = "";
        private string _idBonDeLivraison = "";
        private DateTime _dateReception;
        private string _numeroCompta = "";
        private string _marque = "";
        private string _modele = "";
        private string _descriptions = "";
        private string _idEtat = "";
        private string _recepteur = "";
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
        public string IdBonDeLivraison
        {
            get { return _idBonDeLivraison; }
            set { _idBonDeLivraison = value; }
        }
        public DateTime DateReception
        {
            get { return _dateReception; }
            set { _dateReception = value; }
        }
        public string NumeroCompta
        {
            get { return _numeroCompta; }
            set { _numeroCompta = value; }
        }
        public string Marque
        {
            get { return _marque; }
            set { _marque = value; }
        }
        public string Modele
        {
            get { return _modele; }
            set { _modele = value; }
        }
        public string Descriptions
        {
            get { return _descriptions; }
            set { _descriptions = value; }
        }
        public string IdEtat
        {
            get { return _idEtat; }
            set { _idEtat = value; }
        }
        public string Recepteur
        {
            get { return _recepteur; }
            set { _recepteur = value; }
        }
        public int Statut
        {
            get { return _statut; }
            set { _statut = value; }
        }

        public PvReception()
        {
        }

        public PvReception(string id,string idImmobilier, string idBonDeLivraison,DateTime dateReception,string numeroCompta,string marque,
            string modele,string descriptions,string idEtat,string recepteur, int statut)
        {
            Id = id;
            IdImmobilier = idImmobilier;
            IdBonDeLivraison = idBonDeLivraison;
            DateReception = dateReception;
            NumeroCompta = numeroCompta;
            Marque = marque;
            Modele = modele;
            Descriptions = descriptions;
            IdEtat = idEtat;
            Recepteur = recepteur;
            Statut = statut;
        }
        public PvReception(string idImmobilier, string idBonDeLivraison, DateTime dateReception, string numeroCompta, string marque,
            string modele, string descriptions, string idEtat, string recepteur, int statut)
        {
            IdImmobilier = idImmobilier;
            IdBonDeLivraison = idBonDeLivraison;
            DateReception = dateReception;
            NumeroCompta = numeroCompta;
            Marque = marque;
            Modele = modele;
            Descriptions = descriptions;
            IdEtat = idEtat;
            Recepteur = recepteur;
            Statut = statut;
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

            string sql = "INSERT INTO Pvreception(id,idImmobilier,idbondelivraison,dateReception," +
                "numeroCompta,marque,modele,descriptions,idEtat,recepteur, statut) VALUES(@id,@idImmobilier," +
                "@idbondelivraison,@dateReception,@numeroCompta,@marque,@modele,@descriptions,@idEtat," +
                "@recepteur,@statut)";

            int nextval = Connex.getNextVal(cnx, "pvreception_sequence");

            string newid = this.Id;
            if (String.IsNullOrWhiteSpace(newid))
            {
                newid = Connex.createSequenceId("PVR", 7, nextval);
            }
            if (String.IsNullOrWhiteSpace(this.Descriptions))
            {
                this.Descriptions = "";
            }

            string numeroCompta = this.NumeroCompta + nextval;
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", newid);
                command.Parameters.AddWithValue("@idImmobilier", this.IdImmobilier);
                command.Parameters.AddWithValue("@idbondelivraison", this.IdBonDeLivraison);
                command.Parameters.AddWithValue("@dateReception", this.DateReception);
                command.Parameters.AddWithValue("@numeroCompta", numeroCompta);
                command.Parameters.AddWithValue("@marque", this.Marque);
                command.Parameters.AddWithValue("@modele", this.Modele);
                command.Parameters.AddWithValue("@descriptions", this.Descriptions);
                command.Parameters.AddWithValue("@idEtat", this.IdEtat);
                command.Parameters.AddWithValue("@recepteur", this.Recepteur);
                command.Parameters.AddWithValue("@statut", this.Statut);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static int UpdateStatut(NpgsqlConnection cnx, string idPvReception, int statut)
        {
            int affectedRow = 0;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "UPDATE PvReception SET statut = @newstatut WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@newstatut", statut);
                command.Parameters.AddWithValue("@id", idPvReception);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static PvReception? GetPvReception(NpgsqlConnection cnx, string id)
        {
            PvReception? pvReception = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM Pvreception WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string idImmobilier = reader.GetString(1);
                            string idBonDeLivraison = reader.GetString(2);
                            DateTime dateReception = reader.GetDateTime(3);
                            string numeroCompta = reader.GetString(4);
                            string marque = reader.GetString(5);
                            string modele = reader.GetString(6);
                            string descriptions = reader.GetString(7);
                            string idEtat = reader.GetString(8);
                            string recepteur = reader.GetString(9);
                            int statut = reader.GetInt32(10);

                            pvReception = new PvReception(id, idImmobilier, idBonDeLivraison, dateReception, numeroCompta, marque,
                                modele, descriptions, idEtat, recepteur, statut);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET PVRECEPTION BY ID HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return pvReception;
        }

        public static List<PvReception> GetPvReceptions(NpgsqlConnection cnx, string idCatImmo, int statut)
        {
            List<PvReception> pvReceptions = new List<PvReception>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_pvreception WHERE idcatimmo = @idcatimmo AND statut = @statut";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idcatimmo", idCatImmo);
                command.Parameters.AddWithValue("@statut", statut);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        PvReception? pvReception = null;
                        while (reader.Read())
                        {
                            string id = reader.GetString(0);
                            string idImmobilier = reader.GetString(1);
                            string idBonDeLivraison = reader.GetString(3);
                            DateTime dateReception = reader.GetDateTime(4);
                            string numeroCompta = reader.GetString(5);
                            string marque = reader.GetString(6);
                            string modele = reader.GetString(7);
                            string descriptions = reader.GetString(8);
                            string idEtat = reader.GetString(9);
                            string recepteur = reader.GetString(10);

                            pvReception = new PvReception(id, idImmobilier, idBonDeLivraison, dateReception, numeroCompta, marque,
                                modele, descriptions, idEtat, recepteur, statut);

                            pvReceptions.Add(pvReception);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET PVRECEPTION BY ID HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return pvReceptions;
        }

        public static PvUtilisation? getFirstPvUtilisation(NpgsqlConnection cnx, string idPvReception)
        {
            PvUtilisation? pvUtilisation = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_PvUtilisation WHERE idpvreception=@idpvreception ORDER BY dateDebutUtilisation" +
                " ASC,id ASC LIMIT 1";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idpvreception", idPvReception);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pvUtilisation = new PvUtilisation(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2)
                                , reader.GetString(3), reader.GetString(4), reader.GetString(5));
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET FIRST PVUTILISATION HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return pvUtilisation;
        }

        public static DateTime getDatePremiereUtilisation(NpgsqlConnection cnx, string idPvReception)
        {
            DateTime datePremiere = DateTime.Now;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT dateDebutUtilisation FROM Pvutilisation WHERE idpvreception=@idpvreception " +
                "ORDER BY dateDebutUtilisation ASC LIMIT 1";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idpvreception", idPvReception);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            datePremiere = reader.GetDateTime(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET FIRST PVUTILISATION HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return datePremiere;
        }

        public static DateTime getDateDerniereUtilisation(NpgsqlConnection cnx, string idPvReception)
        {
            DateTime datePremiere = new DateTime(1940,01,01);

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT dateDebutUtilisation FROM Pvutilisation WHERE idpvreception=@idpvreception " +
                "ORDER BY dateDebutUtilisation DESC, id DESC LIMIT 1";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idpvreception", idPvReception);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            datePremiere = reader.GetDateTime(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET FIRST PVUTILISATION HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return datePremiere;
        }

        public List<PvUtilisation> getPvUtilisations(NpgsqlConnection cnx)
        {
            List<PvUtilisation> pvUtilisations = new List<PvUtilisation>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM Pvutilisation WHERE idpvreception = @idpvreception";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idpvreception", this.Id);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        PvUtilisation? pvUtilisation = null;
                        while (reader.Read())
                        {
                            pvUtilisation = new PvUtilisation(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2)
                                , reader.GetString(3), reader.GetString(4));

                            pvUtilisations.Add(pvUtilisation);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET LIST PVUTILISATION BY IDRECEPTION HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return pvUtilisations;
        }

        public static PvUtilisation? getLastPvUtilisation(NpgsqlConnection cnx, string idPvReception)
        {
            PvUtilisation? pvUtilisation = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_Pvutilisation WHERE idpvreception = @idpvreception ORDER BY datedebututilisation DESC, id DESC LIMIT 1";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idpvreception", idPvReception);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pvUtilisation = new PvUtilisation(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2)
                                , reader.GetString(3), reader.GetString(4), reader.GetString(5));
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET LAST PVUTILISATION BY IDRECEPTION HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return pvUtilisation;
        }

        public List<Amortissement> getAmortissement(NpgsqlConnection cnx, InfoAmortissement infoAmortissement)
        {
            List<Amortissement> amortissements = new List<Amortissement>();
            


            return amortissements;
        }
    }
}
