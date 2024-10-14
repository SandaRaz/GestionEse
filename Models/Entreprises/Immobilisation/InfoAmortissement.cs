using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Immobilisation
{
    public class InfoAmortissement
    {
        private string _id;
        private string _idPvReception = "";
        private double _valeurDuBien;
        private double _jourAnnee;
        private double _tauxAmortissementAnnuel;
        private double _tauxDegressif;

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
        public double ValeurDuBien
        {
            get { return _valeurDuBien; }
            set { _valeurDuBien = value; }
        }
        public double JourAnnee
        {
            get { return _jourAnnee; }
            set { _jourAnnee = value; }
        }
        public double TauxAmortissementAnnuel
        {
            get { return _tauxAmortissementAnnuel; }
            set { _tauxAmortissementAnnuel = value; }
        }
        public double TauxDegressif
        {
            get { return _tauxDegressif; }
            set { _tauxDegressif = value; }
        }

        public InfoAmortissement()
        {
        }

        public InfoAmortissement(string id, string idpvreception, double valeurDuBien, double jourAnnee, double tauxAmortAnnuel, double tauxDegressif)
        {
            Id = id;
            IdPvReception = idpvreception;
            ValeurDuBien = valeurDuBien;
            JourAnnee = jourAnnee;
            TauxAmortissementAnnuel = tauxAmortAnnuel;
            TauxDegressif = tauxDegressif;
        }
        public InfoAmortissement(string idpvreception,double valeurDuBien,double jourAnnee,double tauxAmortAnnuel,double tauxDegressif)
        {
            IdPvReception = idpvreception;
            ValeurDuBien = valeurDuBien;
            JourAnnee = jourAnnee;
            TauxAmortissementAnnuel = tauxAmortAnnuel;
            TauxDegressif = tauxDegressif;
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

            String sql = "INSERT INTO Infoamortissement(id,idpvreception,valeurdubien,jourannee," +
                "tauxamortissementannuel,tauxdegressif) VALUES(@id,@idpvreception,@valeurdubien,@jourannee," +
                "@tauxamortannuel,@tauxdegressif)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                string newid = "";
                if (this.Id == null)
                {
                    newid = Connex.createId(cnx, "infoamortissement_sequence", "PVR", 7);
                }
                else
                {
                    newid = this.Id;
                }
                command.Parameters.AddWithValue("@id", newid);
                command.Parameters.AddWithValue("@idpvreception", this.IdPvReception);
                command.Parameters.AddWithValue("@valeurdubien", this.ValeurDuBien);
                command.Parameters.AddWithValue("@jourannee", this.JourAnnee);
                command.Parameters.AddWithValue("@tauxamortannuel", this.TauxAmortissementAnnuel);
                command.Parameters.AddWithValue("@tauxdegressif", this.TauxDegressif);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static InfoAmortissement? getInfoAmortissement(NpgsqlConnection cnx, string idPvReception)
        {
            InfoAmortissement? infoAmortissement = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM Infoamortissement WHERE idpvreception=@idpvreception ";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@idpvreception", idPvReception);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            infoAmortissement = new InfoAmortissement(reader.GetString(0),reader.GetString(1),
                                reader.GetDouble(2),reader.GetDouble(3),reader.GetDouble(4),reader.GetDouble(5));
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET INFOAMORTISSEMENT HAS 0 ROWS");
                    }
                }
            }
            if (closed)
            {
                cnx.Close();
            }
            return infoAmortissement;
        }
    }
}
