using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQPublish
{
    class Program
    {
        ////创建用于链接RabbitMQ服务器器URI
        static Uri connUri = new Uri("amqp://guest:guest123@47.100.193.29:5672/");
        static void Main(string[] args)
        {
            RPCServer();
        }

        public static void PublishQueue()
        {
            
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = connUri;
            //创建链接，与服务器进行连接
            IConnection conn = factory.CreateConnection();
            //创建管道，用于执行基于AMQP 0-9-1协议的管道类。
            IModel channel = conn.CreateModel();
            //通过管道，声明队列名字为testQueue队列，
           
            channel.QueueDeclare(queue: "testQueue",
                                 durable: true,    //durable声明队列是否持久
                                 exclusive: false, //独占，只能有一个连接使用该队列，当连接关闭时，队列自动删除
                                 autoDelete: false,//自动删除，是否最后一个消费者不再订阅时自动删除
                                 arguments: null); //参数， 可选

            //properties 还可以添加一些协议的细节。
            //此处声明消息是持久化消息
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            //向RabbitMQ 下的testQueue队列发送100条消息
            for (int i = 0; i < 100; i++)
            {
                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!" + i);
                channel.BasicPublish(exchange: "",
                                 routingKey: "testQueue",
                                 basicProperties: properties,
                                 body: messageBodyBytes);
            }
            channel.Close();
            conn.Close();
        }

        /// <summary>
        /// PublishSubscribe
        /// 发布订阅，
        /// 生产者（发布者）把消息直接发送到交换机
        /// 声明订阅者队列，并跟交换机进行绑定。
        /// 交换机把同一消息发送到 所有订阅的队列。
        /// 消费者去消费队列里面的信息
        /// 注意：此交换机类型为Fanout扇形交换机
        /// </summary>
        public static void PublishSubscribe()
        {
            var factory = new ConnectionFactory() { Uri = connUri };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
                var message = "PublishExchange";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// 以日志级别作为路由信息发送一条消息到交换机[x]
        /// 此消息会发送给此交换机下，多个拥有对应路由信息的队列里
        /// 一个队列可以标识多种路由信息。
        /// 注意：此交换机类型为direct 直接交换机
        /// </summary>
        public static void Routing()
        {
            var factory = new ConnectionFactory() { Uri = connUri };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                                        type: ExchangeType.Direct);
                Console.WriteLine("输入日志级别");
                var logLevel = Console.ReadLine();
                string message = $"这是一条[{logLevel}]日志";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: logLevel,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", logLevel, body);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static void RPCServer()
        {
            var factory = new ConnectionFactory() { Uri = connUri };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        int n = int.Parse(message);
                        Console.WriteLine(" [.] fib({0})", message);
                        response = fib(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        private static int fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }


    }
}
