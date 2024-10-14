using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class PvUtilisation
    {
        public string _id = "";
        public string _idPvReception = "";
        public DateTime _dateDebutUtilisation;
        public string _utilisateurActuel = "";
        public string _idEtatActuel = "";
        public string _etat = "";

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string IdPvReception
        {
            get { return _idPvReception; }
            set { _idPvReception = value; }
        }
        public DateTime DateDebutUtilisation
        {
            get { return _dateDebutUtilisation; }
            set { _dateDebutUtilisation = value; }
        }
        public string UtilisateurActuel
        {
            get { return _utilisateurActuel; }
            set { _utilisateurActuel = value; }
        }
        public string IdEtatActuel
        {
            get { return _idEtatActuel; }
            set { _idEtatActuel = value; }
        }
        public string Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }

        public PvUtilisation()
        {
        }

        public PvUtilisation(string id,string idPvReception,DateTime dateDebutUtilisation,string utilisateurActuel,string idEtatActuel)
        {
            Id = id;
            IdPvReception = idPvReception;
            DateDebutUtilisation = dateDebutUtilisation;
            UtilisateurActuel = utilisateurActuel;
            IdEtatActuel = idEtatActuel;
        }
        public PvUtilisation(string idPvReception, DateTime dateDebutUtilisation, string utilisateurActuel, string idEtatActuel)
        {
            IdPvReception = idPvReception;
            DateDebutUtilisation = dateDebutUtilisation;
            UtilisateurActuel = utilisateurActuel;
            IdEtatActuel = idEtatActuel;
        }

        public PvUtilisation(string id, string idPvReception, DateTime dateDebutUtilisation, string utilisateurActuel, string idEtatActuel, string etat)
        {
            Id = id;
            IdPvReception = idPvReception;
            DateDebutUtilisation = dateDebutUtilisation;
            UtilisateurActuel = utilisateurActuel;
            IdEtatActuel = idEtatActuel;
            Etat = etat;
        }

        public PvUtilisation(string idPvReception, DateTime dateDebutUtilisation, string utilisateurActuel, string idEtatActuel, string etat)
        {
            IdPvReception = idPvReception;
            DateDebutUtilisation = dateDebutUtilisation;
            UtilisateurActuel = utilisateurActuel;
            IdEtatActuel = idEtatActuel;
            Etat = etat;
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

            String sql = "INSERT INTO Pvutilisation(id,idpvreception,datedebututilisation,utilisateuractuel,idetatactuel) " +
                "VALUES(@id,@idpvreception,@datedebututilisation,@utilisateuractuel,@idetatactuel)";

            string newid = this.Id;
            if (String.IsNullOrWhiteSpace(newid))
            {
                newid = Connex.createId(cnx, "pvutilisation_sequence", "PVU", 7);
            }
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", newid);
                command.Parameters.AddWithValue("@idpvreception", this.IdPvReception);
                command.Parameters.AddWithValue("@datedebututilisation", this.DateDebutUtilisation);
                command.Parameters.AddWithValue("@utilisateuractuel", this.UtilisateurActuel);
                command.Parameters.AddWithValue("@idetatactuel", this.IdEtatActuel);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static List<PvUtilisation> getPvUtilisations(NpgsqlConnection cnx, string idPvReception)
        {
            List<PvUtilisation> pvUtilisations = new List<PvUtilisation>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_Pvutilisation WHERE idpvreception=@idpvreception";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idpvreception", idPvReception);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        PvUtilisation? pvUtilisation = null;
                        while (reader.Read())
                        {
                            pvUtilisation = new PvUtilisation(reader.GetString(0),reader.GetString(1),reader.GetDateTime(2)
                                ,reader.GetString(3),reader.GetString(4), reader.GetString(5));

                            pvUtilisations.Add(pvUtilisation);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET PVUTILISATIONS BY ID HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return pvUtilisations;
        }

        public static PvUtilisation? getPvUtilisation(NpgsqlConnection cnx, string id)
        {
            PvUtilisation? pvUtilisation = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM v_Pvutilisation WHERE id=@id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@id", id);
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
                        Console.WriteLine("READER GET PVUTILISATIONS BY ID HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return pvUtilisation;
        }
    }
}
