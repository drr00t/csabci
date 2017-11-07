using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Core.Logging;

using Tendermint.Abci.Types;

namespace Tendermint.Abci.Examples.Counter
{
    class Program
    {
        const Int32 Port = 46658;
        static void Main(string[] args)
        {

            //// Server Startup
            //GrpcEnvironment.SetLogger(new ConsoleLogger()); // show inner log

            //Server server = new Server
            //{
            //    Services = { ABCIApplication.BindService(new CounterApp()) },
            //    Ports = { new ServerPort( "127.0.0.1", Port, ServerCredentials.Insecure) }
            //};
            //server.Start();

            Console.WriteLine("CounterApp server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");

            new Tendermint.Abci.Servers.AbciSocketServer(46658).Start(); //.Wait();

            Console.Read();

            //server.ShutdownAsync().Wait();
        }
    }
}
