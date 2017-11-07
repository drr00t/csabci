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
    public class TcSocketConnection
    {
        private bool alreadyNamed = false;
        public TcpClient Connection { get; private set; } 
        public ConnectionTypes ConnectionType { get; set; }

        public enum ConnectionTypes { NoNamed = 0x00, Consensus = 0x01, Query = 0x02, Mempool = 0x03, }

        public Task ReceiveTask { get; private set;}

        public TcSocketConnection(TcpClient connection)
        {
            Connection = connection;
            ConnectionType = ConnectionTypes.NoNamed;
            ReceiveTask = Task.FromResult(HandleRequest());
        }


        public Request HandleRequest()
        {
            var inputStream = new CodedInputStream(Connection.GetStream());
            var outputStream = new CodedOutputStream(Connection.GetStream());

            while (Connection.Connected)
            {
                Int32 varintLength = inputStream.ReadLength();

                Console.WriteLine("message length: {0}", varintLength);

                // if (varintLength > 4)
                // {
                //     throw new System.ArgumentOutOfRangeException("varint");
                // }

                //try
                //{
                    var request = Request.Parser.ParseFrom(inputStream.ReadBytes());
                    //Console.WriteLine("New client message: {0} type {1}", request.CalculateSize(), request.ValueCase);
                    outputStream.WriteBytes(inputStream.ReadBytes());
                    UpdateConnectionName(request);

                return request;
                //return request;
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

            }

            return null;
        }

        public void UpdateConnectionName(Request request)
        {
            switch(request.ValueCase)
            {
                case Request.ValueOneofCase.CheckTx:
                    ConnectionType = ConnectionTypes.Mempool;
                    alreadyNamed = true;
                    break;
                case Request.ValueOneofCase.Commit:
                case Request.ValueOneofCase.DeliverTx:
                    ConnectionType = ConnectionTypes.Consensus;
                    alreadyNamed = true;
                    break;
                case Request.ValueOneofCase.Query:
                case Request.ValueOneofCase.Info:
                    ConnectionType = ConnectionTypes.Query;
                    alreadyNamed = true;
                    break;
            }

            Console.WriteLine("Connection name : {0} type {1}", request.CalculateSize(), ConnectionType);
        }
    }    
}

