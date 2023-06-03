using MilitariaRabit.Helper;

namespace MilitariaRabit.Model
{
    public class Email
    {
        public int Id { get; set; }
        public string? Sender { get; set; } = "Sender@test.com";
        public string? Name { get; set; } = "Name";
        public string? Receiver { get; set; } = "Receiver@test.com";
        public string? Title { get; set; }="Title";
        public string? Description { get; set; }="Description";
        public Enums.TypOfEmail TypOfEmail { get; set; } = Enums.TypOfEmail.SmtpClient;

        public static string SerializeObject(Email email)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(email);
        }
        public static Email? DeserializeObject(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Email>(json);
        }
    }

}
