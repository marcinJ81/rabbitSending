using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RAbbitRecive
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            Console.WriteLine("Wysyłacz");
            await channel.QueueDeclareAsync(queue: "task_queue", durable: true, exclusive: false,
                autoDelete: false, arguments: null);

            var message = GetMessage();
            var sendingMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(sendingMessage);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "task_queue", mandatory: true,
                basicProperties: properties, body: body);
            Console.WriteLine($" [x] Sent {message}");
        }

        static List<Mtg> GetMessage()
        {
            List<Mtg> meassegeToSent = new List<Mtg>();

           
            while (true)
            {
                Console.WriteLine("jak chcesz zakończyć i wysłać,  wpisz q, podaj k żeby kontynuować");
                var exit = Console.ReadLine();
                if (exit == "q")
                {
                    break;
                }
                Mtg element = new Mtg();
                Console.WriteLine("Podaj nazwe pola do wysłania");
                element.Name = Console.ReadLine();
                Console.WriteLine("Czy wiadomość jest poprawna? wpisz t lub n.");
                element.IsError = Console.ReadLine().ToUpper() == "t".ToUpper();
                meassegeToSent.Add(element);
            }
            return meassegeToSent;
            //return ((args.Length > 0) ? string.Join(" ", args) : "Hello.W.o.r.l.d.!");
        }
    }
}