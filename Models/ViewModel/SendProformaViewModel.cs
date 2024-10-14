namespace SIProject.Models.ViewModel
{
    public class SendProformaViewModel
    {
        public string idArticle { get; set; }
        public string nomFournisseur { get; set; }
        public string mailFournisseur { get; set; }
        public string mdpFournisseur { get; set; }
        public string mailClient { get; set; }
        public double stockPossession { get; set; }
        public double prixUnitaire { get; set; }
        public string idUnite { get; set; }
        public DateTime dateEmission { get; set; }
        public DateTime dateExpiration { get; set; }

        public bool infosComplet()
        {
            if (nomFournisseur!=null && mailFournisseur!=null && mailClient!=null && 
                idUnite!=null)
            {
                return true;
            }
            return false;
        }
    }
}
