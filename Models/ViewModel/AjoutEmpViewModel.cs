namespace SIProject.Models.ViewModel
{
    public class AjoutEmpViewModel
    {
        public string idPoste { get; set; }
        public IFormFile? photo { get; set; }
        public string? nom { get; set; }
        public string prenoms { get; set; }
        public DateTime datenaissance { get; set; }
        public string genre { get; set; }
        public string? adresse { get; set; }
        public string email { get; set; }
        public string contact { get; set; }

    }
}
