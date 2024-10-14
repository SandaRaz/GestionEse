namespace SIProject.Models.Entreprises.User.EmailService
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ImapServer { get; set; }
        public int ImapPort { get; set; }
    }
}
