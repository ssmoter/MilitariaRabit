using MilitariaRabit.Helper;
using MilitariaRabit.Model;

try
{
    MilitariaRabit.Rabbit rabbit = new MilitariaRabit.Rabbit();
    rabbit.CreateEqueue(MilitariaRabit.Helper.ListOfQueue.Mail);
    Email email = new Email();

    while (true)
    {
        int selected;
        Console.WriteLine("Wybierz opcje:{0}0-Podaj dane{1}1-defoult{2}2-Zmień kolejkę", Environment.NewLine, Environment.NewLine, Environment.NewLine);
        int.TryParse(Console.ReadLine(), out selected);
        switch (selected)
        {
            case 0:
                {
                    Console.WriteLine("Email wysyłającego");
                    email.Sender = Console.ReadLine();
                    Console.WriteLine("Nazwa wysyłającego");
                    email.Name = Console.ReadLine();
                    Console.WriteLine("Email Odbiorcy");
                    email.Receiver = Console.ReadLine();
                    Console.WriteLine("Tytuł maila");
                    email.Title = Console.ReadLine();
                    Console.WriteLine("Treść maila");
                    email.Description = Console.ReadLine();
                    rabbit.SendMessage(Email.SerializeObject(email));
                    Console.WriteLine("Wiadomość wysłana");
                }
                break;
            case 1:
                {
                    var json = Email.SerializeObject(email);
                    Console.WriteLine(json);
                    rabbit.SendMessage(json);
                }
                break;
            case 2:
                {
                    Console.WriteLine("1-{0}{1}2-{2}{3}3-Anuluj", ListOfQueue.Mail, Environment.NewLine, ListOfQueue.Sms, Environment.NewLine);

                    int listOfQueue;
                    int.TryParse(Console.ReadLine(), out listOfQueue);
                    if (listOfQueue == 1)
                    {
                        rabbit.CloseEqueue();
                        rabbit.CreateEqueue(ListOfQueue.Mail);
                    }
                    if (listOfQueue == 2)
                    {
                        rabbit.CloseEqueue();
                        rabbit.CreateEqueue(ListOfQueue.Sms);
                    }
                }
                break;
            default:
                break;
        }
        Console.Clear();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ForegroundColor = ConsoleColor.Black;
}

