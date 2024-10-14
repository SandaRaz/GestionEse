using MimeKit;

namespace SIProject.Models.Entreprises.User.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(string sender, Message message);
        Task SendEmailAsync(string sender, Message message);
        void ReceiveEmails();
        List<MimeMessage> ReceiveEmails(DateTime dates, string subjectFilter);
    }
}
