using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Consumer_Console
{
    class Program
    {
        static Uri connUrl = new Uri("amqp://guest:guest123@47.100.193.29:5672/");
        static ConnectionFactory factory = new ConnectionFactory() { Uri = connUrl, AutomaticRecoveryEnabled = true };
        static IConnection connection = null;
        static IModel channel = null;
        static void Main(string[] args)
        {

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: "testQueue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);//durable声明堆栈是否持久
            channel.BasicQos(0, 1, false);//未处理完成之前不要在发送新消息给此消费者
           
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: "testQueue",
                               autoAck: false,
                               consumer: consumer);//取消自动确认
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            consumer.Received += (model, ea) =>
            {
                //Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(" [x] Received {0}", message);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);//手动确认
            };

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

    }
}
