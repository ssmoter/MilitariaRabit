using MilitariaRabit.Model;

using System.Net.Mail;

namespace MilitariaRabitClient
{
    public class Smtp
    {
        private SmtpClient _client;

        public Smtp()
        {
            _client = new SmtpClient();
        }

        public async Task SendSmtp(Email email)
        {
            try
            {
                MailAddress from = new MailAddress(email.Sender, email.Name);
                MailAddress to = new MailAddress(email.Receiver);

                MailMessage message = new MailMessage();
                message.From = from;
                message.To.Add(to);
                message.Subject = email.Title;
                message.Body = email.Description;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                await _client.SendMailAsync(message);
                message.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            finally { _client.Dispose(); }

        }


    }
}
