using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;

namespace SIProject.Models.Entreprises.User.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(string sender, Message message)
        {
            var emailMessage = CreateEmailMessage(sender, message);
            Send(emailMessage);
        }

        public async Task SendEmailAsync(string sender, Message message)
        {
            var emailMessage = CreateEmailMessage(sender, message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(string sender, Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(sender, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch(Exception e)
                {
                    Console.Error.WriteLine("ERROR: " +e);
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        public void ReceiveEmails()
        {
            using (var client = new ImapClient())
            {
                client.Connect(_emailConfig.ImapServer, _emailConfig.ImapPort, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var subjectResults = inbox.Search(SearchQuery.SubjectContains("Proforma"));
                var dateResults = inbox.Search(SearchQuery.SentSince(new DateTime(2023, 11, 25)));
                var finalResults = subjectResults.Intersect(dateResults);

                Console.WriteLine("Total messages: {0}", finalResults.Count());
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                //for (int i = 0; i < inbox.Count; i++)
                //{
                    //var message = inbox.GetMessage(i);
                    //Console.WriteLine("Subject: {0}", message.Subject);
                    // Process the message as needed
                //}

                client.Disconnect(true);
            }
        }

        public List<MimeMessage> ReceiveEmails(DateTime dates, string subjectFilter)
        {
            List<MimeMessage> messages = new List<MimeMessage>();

            using (var client = new ImapClient())
            {
                client.Connect(_emailConfig.ImapServer, _emailConfig.ImapPort, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var subjectResults = inbox.Search(SearchQuery.SubjectContains(subjectFilter));
                var dateResults = inbox.Search(SearchQuery.SentSince(dates));

                var allResults = subjectResults.Intersect(dateResults);
                var received = inbox.Search(SearchQuery.Not(SearchQuery.FromContains(_emailConfig.UserName)));

                var finalResults = allResults.Intersect(received);

                Console.WriteLine("Total messages: {0}", finalResults.Count());
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                foreach (var index in finalResults)
                {
                    messages.Add(inbox.GetMessage(index));
                }

                client.Disconnect(true);
            }
            return messages;
        }
    }
}
