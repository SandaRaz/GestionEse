namespace SIProject.Models.Entreprises.Immobilisation
{
    public class Amortissement
    {
        private int _annee;
        private double _vncDebutExercice;
        private double _amortissementAnnuel;
        private double _cumulAmortissement;
        private double _vncFinExercice;
        private double _dotationPeriode;

        public int Annee
        {
            get { return _annee; }
            set { _annee = value; }
        }
        public double VncDebutExercice
        {
            get { return _vncDebutExercice; }
            set { _vncDebutExercice = value; }
        }
        public double AmortissementAnnuel
        {
            get { return _amortissementAnnuel; }
            set { _amortissementAnnuel = value; }
        }
        public double CumulAmortissement
        {
            get { return _cumulAmortissement; }
            set { _cumulAmortissement = value; }
        }
        public double VncFinExercice
        {
            get { return _vncFinExercice; }
            set { _vncFinExercice = value; }
        }
        public double DotationPeriode
        {
            get { return _dotationPeriode; }
            set { _dotationPeriode = value; }
        }

        public Amortissement(int annee,double vncdebut,double amortAnnuel,double cumulAmort,double vncFin,double dotationPeriode)
        {
            Annee = annee;
            VncDebutExercice = vncdebut;
            AmortissementAnnuel = amortAnnuel;
            CumulAmortissement = cumulAmort;
            VncFinExercice = vncFin;
            DotationPeriode = dotationPeriode;
        }

        public Amortissement(int annee, double vncdebut, double amortAnnuel, double cumulAmort, double vncFin)
        {
            Annee = annee;
            VncDebutExercice = vncdebut;
            AmortissementAnnuel = amortAnnuel;
            CumulAmortissement = cumulAmort;
            VncFinExercice = vncFin;
        }
    }
}
