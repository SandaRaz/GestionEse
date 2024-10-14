namespace SIProject.Models.Entreprises.Commerce
{
    public class EtatDeStock
    {
        private Article _article;
        private double _quantiteInitiale;
        private double _reste;
        private UniteDetaillee _unite;
        private double _valeur;
        private double _puMoyenne;

        public Article Article
        {
            get { return _article; }
            set { _article = value; }
        }
        public double QuantiteIntitiale
        {
            get { return _quantiteInitiale; }
            set { _quantiteInitiale = value; }
        }
        public double Reste
        {
            get { return _reste; }
            set { _reste = value; }
        }
        public UniteDetaillee Unite
        {
            get { return _unite; }
            set { _unite = value; }
        }
        public double Valeur
        {
            get { return _valeur; }
            set { _valeur = value; }
        }
        public double PuMoyenne
        {
            get { return _puMoyenne; }
            set { _puMoyenne = value; }
        }

        public EtatDeStock()
        {
        }

        public EtatDeStock(Article article, double quantiteIntitiale, double reste, UniteDetaillee unite, double valeur, double puMoyenne)
        {
            Article = article;
            QuantiteIntitiale = quantiteIntitiale;
            Reste = reste;
            Unite = unite;
            Valeur = valeur;
            PuMoyenne = puMoyenne;
        }
    }
}
