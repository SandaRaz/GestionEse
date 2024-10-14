namespace SIProject.Models.ViewModel
{
    public class StockageViewModel
    {
        public string idArticle { get; set; }
        public double quantite { get; set; }
        public string uniteQuantite { get; set; }
        public double prixUnitaire { get; set; }
        public string unitePU { get; set; }

        public DateTime dates { get; set; }

        public bool infosComplet()
        {
            if (quantite != null && uniteQuantite != null && prixUnitaire != null &&
                unitePU != null)
            {
                return true;
            }
            return false;
        }
    }
}
