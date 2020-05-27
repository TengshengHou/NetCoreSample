using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Consumer_Console
{
    class ProgramPublishSubscribe
    {
        static Uri connUrl = new Uri("amqp://guest:guest123@47.100.193.29:5672/");

        static void Main(string[] args)
        {
            Console.WriteLine("ProgramWorkQueues.");
            var factory = new ConnectionFactory() { Uri = connUrl };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout); // ExchangeType.Fanout  扇形 生产者直接发送到交换机， 
                var queueName = channel.QueueDeclare().QueueName;//会自动创建一个 非持久， 断开自动删除的堆栈，并返回堆栈名字，
                channel.QueueBind(queue: queueName,
                                  exchange: "logs",
                                  routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
