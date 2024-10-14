namespace SIProject.Models.ViewModel
{
    public class AjoutBDLViewModel
    {
        public string idBonDeCommande { get; set; }
        public double quantiteLivree { get; set; }
        public double prix { get; set; }
        public double frais { get; set; }
        public string descriptions { get; set; }
        public string observations { get; set; }
        public string adresse { get; set; }
        public string livreur { get; set; }
        public string contactLivreur { get; set; }
        public DateTime dateLivraison { get; set; }
    }
}
