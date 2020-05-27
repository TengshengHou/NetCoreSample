using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Consumer_Console
{
    class ProgramSomething
    {
        static Uri connUrl = new Uri("amqp://guest:guest123@47.100.193.29:5672/");
        static void Main(string[] args)
        {

            Console.WriteLine("消费者程序.");
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = connUrl;
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare("testQueue", true, false, false, null);
            bool noAck = false;
            BasicGetResult result = channel.BasicGet("testQueue", noAck);
            if (result != null)
            {
                IBasicProperties props = result.BasicProperties;
                ReadOnlyMemory<byte> body = result.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                channel.BasicAck(result.DeliveryTag, false);
                Console.WriteLine(message);
            }
            channel.Close();
            conn.Close();
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
