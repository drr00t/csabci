using Google.Protobuf;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Servers.Sockets
{
    public class AbciSocket
    {
        TcpListener _listener;
        CancellationToken _cancellation;
        
        static ManualResetEvent readness = new ManualResetEvent(false); 



        public IPEndPoint EndPointListen { get; private set; }

        public AbciSocket(int port)
            :this(new IPEndPoint(IPAddress.Any, port))
        {
        }

        public AbciSocket(IPEndPoint endpoint)
        {
            EndPointListen = endpoint;

            _listener = new TcpListener(EndPointListen);
        }

        public void Accept()
        {
            //return Task.Run(() => {
                    _listener.Start();
                while (true)
                {
                    readness.Reset();

                    var client = _listener.AcceptTcpClientAsync().Result;
                //.ContinueWith<TcSocketConnection>(HandleConnection);

                    HandleConnection(client);

                    readness.WaitOne();
                }
            //});

        }


        private TcSocketConnection HandleConnection(/*Task<*/ TcpClient /*>*/ clientTask)
        {

            //var client = clientTask.Result;

            readness.Set();

            return new TcSocketConnection(clientTask);
        }

        private Task<Request> HandleRequest(TcSocketConnection client)
        {
            var inputStream = new CodedInputStream(client.Connection.GetStream());
            var outputStream = new CodedOutputStream(client.Connection.GetStream());

            while (client.Connection.Connected)
            {
                Int32 varintLength = inputStream.ReadLength();

                Console.WriteLine("message length: {0}", varintLength);

                if (varintLength > 4)
                {
                    throw new System.ArgumentOutOfRangeException("varint");
                }

                try
                {
                    var request = Request.Parser.ParseFrom(inputStream.ReadBytes());
                    Console.WriteLine("message: {0} type {1}", request.CalculateSize(), request.ValueCase);
                    outputStream.WriteBytes(inputStream.ReadBytes());
                }
                catch (Exception ex)
                {

                }
            }
            
            return null;
        }
    }    
}

