using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class DetailImmobilier
    {
        private string _id = "";
        private string _idImmobilier = "";
        private string _immobilier = "";
        private string _idCatImmo = "";
        private string _reference = "";
        private string _fournisseur = "";
        private double _prix;
        private DateTime _dateReception;
        private string _numeroCompta = "";
        private string _marque = "";
        private string _modele = "";
        private string _descriptions = "";
        private string _idEtat = "";
        private string _etat = "";
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
        public string Immobilier
        {
            get { return _immobilier; }
            set { _immobilier = value; }
        }
        public string IdCatImmo
        {
            get { return _idCatImmo; }
            set { _idCatImmo = value; }
        }
        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }
        public string Fournisseur
        {
            get { return _fournisseur; }
            set { _fournisseur = value; }
        }
        public double Prix
        {
            get { return _prix; }
            set { _prix = value; }
        }
        public DateTime dateReception
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
        public string Etat
        {
            get { return _etat; }
            set { _etat = value; }
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

        public DetailImmobilier()
        {
        }

        public DetailImmobilier(string id, string idImmobilier, string immobilier, string idCatImmo, string reference, 
            string fournisseur, double prix, DateTime dateReception, string numeroCompta, string marque, 
            string modele, string descriptions, string idEtat, string etat, string recepteur, int statut)
        {
            Id = id;
            IdImmobilier = idImmobilier;
            Immobilier = immobilier;
            IdCatImmo = idCatImmo;
            Reference = reference;
            Fournisseur = fournisseur;
            Prix = prix;
            this.dateReception = dateReception;
            NumeroCompta = numeroCompta;
            Marque = marque;
            Modele = modele;
            Descriptions = descriptions;
            IdEtat = idEtat;
            Etat = etat;
            Recepteur = recepteur;
            Statut = statut;
        }
        public DetailImmobilier(string idImmobilier, string immobilier, string idCatImmo, string reference,
            string fournisseur, double prix, DateTime dateReception, string numeroCompta, string marque,
            string modele, string descriptions, string idEtat, string etat, string recepteur, int statut)
        {
            IdImmobilier = idImmobilier;
            Immobilier = immobilier;
            IdCatImmo = idCatImmo;
            Reference = reference;
            Fournisseur = fournisseur;
            Prix = prix;
            this.dateReception = dateReception;
            NumeroCompta = numeroCompta;
            Marque = marque;
            Modele = modele;
            Descriptions = descriptions;
            IdEtat = idEtat;
            Etat = etat;
            Recepteur = recepteur;
            Statut = statut;
        }

        public static List<DetailImmobilier> GetDetailImmobiliers(NpgsqlConnection cnx, string idCatImmo, int statut)
        {
            List<DetailImmobilier> immobiliers = new List<DetailImmobilier>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM DetailImmobilier WHERE idCatImmo = @idcatimmo AND statut = @statut";
            using(NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idcatimmo", idCatImmo);
                command.Parameters.AddWithValue("@statut", statut);

                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        DetailImmobilier? detailImmobilier = null;
                        while (reader.Read())
                        {
                            string id = reader.GetString(0);
                            string idimmobilier = reader.GetString(1);
                            string immobilier = reader.GetString(2);
                            string idcatimmo = reader.GetString(3);
                            string reference = reader.GetString(4);
                            string fournisseur = reader.GetString(5);
                            double prix = reader.GetDouble(6);
                            DateTime datereception = reader.GetDateTime(7);
                            string numerocompta = reader.GetString(8);
                            string marque = reader.GetString(9);
                            string modele = reader.GetString(10);
                            string descriptions = reader.GetString(11);
                            string idetat = reader.GetString(12);
                            string etat = reader.GetString(13);
                            string recepteur = reader.GetString(14);

                            detailImmobilier = new DetailImmobilier(id, idimmobilier, immobilier, idcatimmo,
                                reference, fournisseur, prix, datereception, numerocompta, marque, modele,
                                descriptions, idetat, etat, recepteur, statut);

                            immobiliers.Add(detailImmobilier);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET DETAILIMMOBILIERS HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return immobiliers;
        }

        public static DetailImmobilier? GetDetailImmobilier(NpgsqlConnection cnx, string id)
        {
            DetailImmobilier? detailImmobilier = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM DetailImmobilier WHERE id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string idimmobilier = reader.GetString(1);
                            string immobilier = reader.GetString(2);
                            string idcatimmo = reader.GetString(3);
                            string reference = reader.GetString(4);
                            string fournisseur = reader.GetString(5);
                            double prix = reader.GetDouble(6);
                            DateTime datereception = reader.GetDateTime(7);
                            string numerocompta = reader.GetString(8);
                            string marque = reader.GetString(9);
                            string modele = reader.GetString(10);
                            string descriptions = reader.GetString(11);
                            string idetat = reader.GetString(12);
                            string etat = reader.GetString(13);
                            string recepteur = reader.GetString(14);
                            int statut = reader.GetInt32(15);

                            detailImmobilier = new DetailImmobilier(id, idimmobilier, immobilier, idcatimmo,
                                reference, fournisseur, prix, datereception, numerocompta, marque, modele,
                                descriptions, idetat, etat, recepteur, statut);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GET DETAILIMMOBILIERS HASN'T ANY ROW");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return detailImmobilier;
        }

        public static List<Amortissement> getAmortissementLineaire(NpgsqlConnection cnx, string idPvReception,
            double annuiteAmortissement)
        {
            List<Amortissement> amortissements = new List<Amortissement>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            PvUtilisation premierUtilisation = PvReception.getFirstPvUtilisation(cnx, idPvReception) ?? throw new Exception("GET First Pv Utilisation By IdReception NULL");
            DetailImmobilier detailImmobilier = DetailImmobilier.GetDetailImmobilier(cnx, idPvReception) ?? throw new Exception("GET PvDetailImmobilier By IdReception NULL");


        // Initialiser l'ammortissement lineaire de la premiere annee
            double vncDebut = detailImmobilier.Prix;
            int nombreAnnee = (int)(vncDebut / annuiteAmortissement);
            double amortissementCumulee = annuiteAmortissement;
            double vncFinExercice = vncDebut - annuiteAmortissement;
            Amortissement amortissement = new Amortissement(1, vncDebut, annuiteAmortissement, amortissementCumulee, vncFinExercice);

            amortissements.Add(amortissement);
        //On commence par 1 car la premiere annee a ete deja initialisee
            for(int i = 2; i<=nombreAnnee; i++)
            {
                vncDebut = amortissement.VncFinExercice;
                amortissementCumulee = amortissement.CumulAmortissement + annuiteAmortissement;
                vncFinExercice = vncDebut - annuiteAmortissement;

                amortissement = new Amortissement(i,vncDebut,annuiteAmortissement,amortissementCumulee,vncFinExercice);
                amortissements.Add(amortissement);
            }


            if (closed)
            {
                cnx.Close();
            }
            return amortissements;
        }
    }
}
