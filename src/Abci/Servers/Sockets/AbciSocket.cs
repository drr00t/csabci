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

        public Task Accept()
        {
            return Task.Run(() => {
                    _listener.Start();
                while (true)
                {
                    readness.Reset();

                    _listener.AcceptTcpClientAsync()
                            .ContinueWith<TcSocketConnection>(HandleConnection);

                    readness.WaitOne();
                }
            });

        }


        private TcSocketConnection HandleConnection(Task<TcpClient> clientTask)
        {

            var client = clientTask.Result;

            readness.Set();

            return new TcSocketConnection(client);
        }

        private Task<Request> HandleRequest(TcSocketConnection client)
        {
            var inputStream = new CodedInputStream(client.Connection.GetStream());
            var outputStream = new CodedOutputStream(client.Connection.GetStream());

            //while(client.Connected)
            //{
            Int32 varintLength = inputStream.ReadLength();

            Console.WriteLine("message length: {0}",varintLength);

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
            catch(Exception ex)
            {

            }






            //byte[] msgLengthField = new byte[varintLength];
            //stream.ReadAsync(msgLengthField, 1, varintLength);

            //Int64 msgLengthLong = BitConverter.ToInt64(msgLengthField, 0);

            //if (msgLengthLong > Int32.MaxValue)
            //{
            //    throw new System.ArgumentOutOfRangeException("messageLength");
            //}

            //Int32 messageLength = (Int32)msgLengthLong;
            //CodedInputStream.
            //Request request = Request.Parser.ParseFrom()
            //}




            //if(varintLength)
            return null;
        }

        //private async Task<Request> HandleConnection(TcpClient client)
        //{
        //    var buffer = new byte[8192];

        //    using (var ns = client.GetStream())
        //    {
        //        var byteCount = await ns.ReadAsync(buffer, 0, buffer.Length);
        //        var bs = Google.Protobuf.ByteString.FromStream(ns);

        //        var request = Request.Parser.ParseFrom(bs);

        //        switch (request.ValueCase)
        //        {
        //            case Request.ValueOneofCase.CheckTx:
        //                break;
        //            case Request.ValueOneofCase.DeliverTx:
        //                break;
        //            case Request.ValueOneofCase.Commit:
        //                break;
        //            case Request.ValueOneofCase.BeginBlock:
        //                break;
        //            case Request.ValueOneofCase.EndBlock:
        //                break;
        //            case Request.ValueOneofCase.InitChain:
        //                break;
        //            case Request.ValueOneofCase.Info:
        //                break;
        //            case Request.ValueOneofCase.Query:
        //                break;
        //            case Request.ValueOneofCase.SetOption:
        //                break;

        //        }

        //        Console.WriteLine("[Server] Reading from client");
        //    }
        //}

        //public async Task HandleRequest(Request request)
        //{

        //}
    }    
}

