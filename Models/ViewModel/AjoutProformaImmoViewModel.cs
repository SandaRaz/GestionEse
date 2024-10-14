namespace SIProject.Models.ViewModel
{
    public class AjoutProformaImmoViewModel
    {
        public string idImmobilier { get; set; }
        public double quantite { get; set; }
        public double prix { get; set; }
        public DateTime dateEmission { get; set; }
        public DateTime dateExpiration { get; set; }
        public string idEtat { get; set; }
        public string idFournisseur { get; set; }
        public string fournisseur { get; set; } = "";
        public string reference { get; set; } = "";
    }
}
