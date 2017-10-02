using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Examples
{
    class Program
    {
        const Int32 Port = 46658;
        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { ABCIApplication.BindService(new CounterApp()) },
                Ports = { new ServerPort("127.0.0.1", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.Read();

            server.ShutdownAsync().Wait();
        }
    }
}
