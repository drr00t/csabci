using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Tendermint.Abci.Servers.Socket;
using Xunit;

namespace Abci.Tests
{
    public class AbciSocketTests
    {
        [Fact]
        public void AcceptCOnnections()
        {
            var server = new Tendermint.Abci.Servers.Socket.AbciSocket(46658).Start();

            var client = new TcpClient();

            client.ConnectAsync("127.0.0.1", 46658).Wait();

            client.GetStream().WriteByte(1);

            var buff = new byte[1];
            client.GetStream().ReadAsync(buff,0,1).Wait();
            
            Assert.True(client.Connected);           
        }
    }
}
