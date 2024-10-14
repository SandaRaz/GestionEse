using MimeKit;
using Npgsql;
using SIProject.Models.Cnx;

namespace SIProject.Models.Entreprises.Commerce
{
    public class Mail
    {
        private string _messageId;
        private string _sender;
        private string _senderMail;
        private string _receiver;
        private string _receiverMail;
        private string _subject;
        private string _content;
        private DateTime _dates;
        private int _state;

        public string MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }

        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        public string SenderMail
        {
            get { return _senderMail; }
            set { _senderMail = value; }
        }

        public string Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }
        public string ReceiverMail
        {
            get { return _receiverMail; }
            set { _receiverMail = value; }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        public DateTime Dates
        {
            get { return _dates; }
            set { _dates = value; }
        }
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }

        public Mail()
        {
        }

        public Mail(string messageId, MailboxAddress sender, MailboxAddress receiver, string subject, string content, DateTimeOffset dates, int state)
        {
            MessageId = messageId;
            Sender = sender.Name;
            SenderMail = sender.Address;
            Receiver = receiver.Name;
            ReceiverMail = receiver.Address;
            Subject = subject;
            Content = content;
            DateTimeOffset dateTimeOffset = dates;
            DateTime dateTime = dateTimeOffset.DateTime;
            Dates = dateTime;
            State = state;
        }

        public Mail(string messageId, string sender, string senderMail, string receiver, string receiverMail, string subject, string content, DateTimeOffset dates, int state)
        {
            MessageId = messageId;
            Sender = sender;
            SenderMail = senderMail;
            Receiver = receiver;
            ReceiverMail = receiverMail;
            Subject = subject;
            Content = content;
            DateTimeOffset dateTimeOffset = dates;
            DateTime dateTime = dateTimeOffset.DateTime;
            Dates = dateTime;
            State = state;
        }

        public int save(NpgsqlConnection cnx)
        {
            int affectedRow = 0;
            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            String sql = "INSERT INTO mail(messageid,sender,sendermail,receiver,receivermail,subject,content,dates,state) " +
                "VALUES(@messageid,@sender,@sendermail,@receiver,@receivermail,@subject,@content,@dates,@state)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@messageid", this.MessageId);
                command.Parameters.AddWithValue("@sender", this.Sender);
                command.Parameters.AddWithValue("@sendermail", this.SenderMail);
                command.Parameters.AddWithValue("@receiver", this.Receiver);
                command.Parameters.AddWithValue("@receivermail", this.Sender);
                command.Parameters.AddWithValue("@subject", this.Subject);
                command.Parameters.AddWithValue("@content", this.Content);
                command.Parameters.AddWithValue("@dates", this.Dates);
                command.Parameters.AddWithValue("@state", this.State);

                affectedRow = command.ExecuteNonQuery();
            }

            if (closed)
            {
                cnx.Close();
            }
            return affectedRow;
        }

        public static Mail? getMail(NpgsqlConnection cnx, string messageId)
        {
            Mail? mail = null;

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM mail WHERE messageid=@messageid";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@messageid", messageId);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            mail = new Mail(Convert.ToString(reader["messageid"]), Convert.ToString(reader["sender"]),
                                Convert.ToString(reader["sendermail"]), Convert.ToString(reader["receiver"]), 
                                Convert.ToString(reader["receivermail"]), Convert.ToString(reader["subject"]),
                                Convert.ToString(reader["content"]), Convert.ToDateTime(reader["dates"]),
                                Convert.ToInt32(reader["state"]));
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET MAIL HAS 0 ROWS");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return mail;
        }

        public static List<Mail> getMails(NpgsqlConnection cnx)
        {
            List<Mail> mails = new List<Mail>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM mail";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Mail mail = null;
                        while (reader.Read())
                        {
                            mail = new Mail(Convert.ToString(reader["messageid"]), Convert.ToString(reader["sender"]),
                                Convert.ToString(reader["sendermail"]), Convert.ToString(reader["receiver"]), 
                                Convert.ToString(reader["receivermail"]), Convert.ToString(reader["subject"]),
                                Convert.ToString(reader["content"]), Convert.ToDateTime(reader["dates"]),
                                Convert.ToInt32(reader["state"]));
                            mails.Add(mail);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET MAIL HAS 0 ROWS");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return mails;
        }

        public static List<Mail> getMails(NpgsqlConnection cnx, int state)
        {
            List<Mail> mails = new List<Mail>();

            bool closed = false;
            if (cnx.State == System.Data.ConnectionState.Closed)
            {
                cnx = Connex.getConnection();
                cnx.Open();
                closed = true;
            }

            string sql = "SELECT * FROM mail WHERE state=@state";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, cnx))
            {
                command.Parameters.AddWithValue("@state", state);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Mail mail = null;
                        while (reader.Read())
                        {
                            mail = new Mail(Convert.ToString(reader["messageid"]), Convert.ToString(reader["sender"]),
                                Convert.ToString(reader["sendermail"]), Convert.ToString(reader["receiver"]), 
                                Convert.ToString(reader["receivermail"]), Convert.ToString(reader["subject"]),
                                Convert.ToString(reader["content"]), Convert.ToDateTime(reader["dates"]),
                                Convert.ToInt32(reader["state"]));
                            mails.Add(mail);
                        }
                    }
                    else
                    {
                        Console.WriteLine("READER GET MAILS HAS 0 ROWS");
                    }
                }
            }

            if (closed)
            {
                cnx.Close();
            }
            return mails;
        }

    }
}
