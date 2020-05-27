using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Consumer_Console
{
    class ProgramRouting
    {
        static Uri connUri = new Uri("amqp://guest:guest123@47.100.193.29:5672/");

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { Uri = connUri, AutomaticRecoveryEnabled = true };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())//通道
            {
                //交换机声明
                channel.ExchangeDeclare(exchange: "direct_logs",
                                        type: "direct");
                //堆栈声明
                var queueName = channel.QueueDeclare().QueueName;


                //args = new string[] { "error", "info" };
                var inputText = Console.ReadLine();
                var severitys = inputText.Split(',');

                #region 记录Info与Error日志
                foreach (var severity in severitys)
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: "direct_logs",
                                      routingKey: severity);
                }
                #endregion


                #region MyRegion
                //channel.QueueBind(queue: queueName,
                //                  exchange: "direct_logs",
                //                  routingKey: args[0]); 
                #endregion

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                                      routingKey, message);
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
