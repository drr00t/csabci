using Google.Protobuf;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Servers.Socket
{
    public class TcSocketConnection
    {
        public TcpClient Connection { get; private set; } 
        public ConnectionTypes ConnectionType { get; set; }

        public enum ConnectionTypes { NoNamed = 0x00, Consensus = 0x01, Query = 0x02, Mempool = 0x03, }

        public Task ReceiveTask { get; private set;}

        public TcSocketConnection(TcpClient connection)
        {
            Connection = connection;
            ConnectionType = ConnectionTypes.NoNamed;
            ReceiveTask = Task.Run(HandleRequest);
        }


        public Task<Request> HandleRequest()
        {
            var inputStream = new CodedInputStream(Connection.GetStream());
            var outputStream = new CodedOutputStream(Connection.GetStream());

            //while(client.Connected)
            //{
            Int32 varintLength = inputStream.ReadLength();

            Console.WriteLine("message length: {0}",varintLength);

            // if (varintLength > 4)
            // {
            //     throw new System.ArgumentOutOfRangeException("varint");
            // }

            try
            {
                var request = Request.Parser.ParseFrom(inputStream.ReadBytes());
                Console.WriteLine("New client message: {0} type {1}", request.CalculateSize(), request.ValueCase);
                outputStream.WriteBytes(inputStream.ReadBytes());
            }
            catch(Exception ex)
            {
                
            }

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

