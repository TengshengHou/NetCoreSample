using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ_Consumer_Console.Rpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Consumer_Console
{
    class ProgramRpc
    {
        static Uri connUrl = new Uri("amqp://guest:guest123@47.100.193.29:5672/");
       
        static void Main(string[] args)
        {



            var rpcClient = new RpcClient();

            Console.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30");

            Console.WriteLine(" [.] Got '{0}'", response);
            rpcClient.Close();
        }

    }
}
