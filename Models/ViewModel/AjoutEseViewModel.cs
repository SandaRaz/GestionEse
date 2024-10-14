namespace SIProject.Models.ViewModel
{
    public class AjoutEseViewModel
    {
        public string nom { get; set; }
        public string? email { get; set; }
        public string objet { get; set; }
        public string siege { get; set; }
        public string? chef { get; set; }
        public string? nif { get; set; }
        public string? numstat { get; set; }
        public string? status { get; set; } 
        public DateTime datecreaction { get; set; }
        public IFormFile logo { get; set; }
        public string mdp { get; set; }
        public string confirmmdp { get; set; }

        public void DisplayViewModel()
        {
            Console.WriteLine($"Nom: {nom} \n Email: {email} \n Objet: {objet} Siege: {siege} \n Chef: {chef} \n Nif: {nif} Status: {status} \n Date de Creation: {datecreaction} Logo: {logo.FileName}");
        }
    }
}
