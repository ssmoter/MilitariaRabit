using MilitariaRabit;
using MilitariaRabit.Helper;
using MilitariaRabit.Model;

using MilitariaRabitClient;

try
{

    Rabbit rabbit = new Rabbit();
    rabbit.CreateEqueue(ListOfQueue.Mail);

    Action<string> messageF = async (string message) =>
    {
        var email = MilitariaRabit.Model.Email.DeserializeObject(message);
        if (email != null)
        {
            await ChoseClient(email);
        }
#if DEBUG 
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Black;
#endif
    };

    rabbit.GetMessage(ListOfQueue.Mail, messageF);


    Console.ReadKey();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ForegroundColor = ConsoleColor.Black;
}

async Task ChoseClient(Email email)
{
    switch (email.TypOfEmail)
    {
        case Enums.TypOfEmail.SmtpClient:
            {
                Smtp smtp = new Smtp();
                await smtp.SendSmtp(email);
            }
            break;
        case Enums.TypOfEmail.AnotherClient:
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Niezaimplementowana opcja");
                Console.ForegroundColor = ConsoleColor.Black;
            }
            break;
        default:
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nieobsługiwana opcja");
                Console.ForegroundColor = ConsoleColor.Black;
            }
            break;
    }

}