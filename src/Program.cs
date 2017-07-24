using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace src
{
    class Program
    {
        const Int32 Port = 46658;
        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { Tendermint.ABCI.Types.ABCIApplication.BindService(new ABCIApp()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }

    class ABCIApp: Tendermint.ABCI.Types.ABCIApplication.ABCIApplicationBase{
        private int hashCount = 0;
        private int txCount = 0;

        public override Task<Tendermint.ABCI.Types.ResponseCommit> Commit(Tendermint.ABCI.Types.RequestCommit request, Grpc.Core.ServerCallContext context)
        {
            hashCount += 1;

            if(txCount == 0)
            {
                return Task.FromResult(new Tendermint.ABCI.Types.ResponseCommit{
                    Code = Tendermint.ABCI.Types.CodeType.Ok
                });    
            }
            else
            {
                using(System.IO.MemoryStream data = new System.IO.MemoryStream())
                {
                    var bd = System.Text.Encoding.UTF8.GetBytes(txCount.ToString());
                    data.Write(bd,0,bd.Length-1);

                    return Task.FromResult(new Tendermint.ABCI.Types.ResponseCommit{
                        Code = Tendermint.ABCI.Types.CodeType.Ok,
                        Data =  Google.Protobuf.ByteString.FromStream(data) 
                    });
                }                
            }
        }

        public override Task<Tendermint.ABCI.Types.ResponseDeliverTx> DeliverTx(Tendermint.ABCI.Types.RequestDeliverTx request, Grpc.Core.ServerCallContext context)
        {
            var tx = request.Tx;
            var data = tx.ToBase64();

            Console.WriteLine("DeliverTx: ${data}");

            if(tx.Length == 0)
            {
                return Task.FromResult(new Tendermint.ABCI.Types.ResponseDeliverTx {
                    Code = Tendermint.ABCI.Types.CodeType.BadNonce,
                    Log = "Tansaction is emptyr"
                });
            }
            else if(tx.Length <= 4)
            {
                int x = Int32.Parse(tx.ToString());

                if(x != txCount)
                {
                    return Task.FromResult(new Tendermint.ABCI.Types.ResponseDeliverTx {
                        Code = Tendermint.ABCI.Types.CodeType.BadNonce,
                        Log = "Invalid nonce"
                    });    
                }
            }
            else
            {
                return Task.FromResult(new Tendermint.ABCI.Types.ResponseDeliverTx
                {
                    Code = Tendermint.ABCI.Types.CodeType.BadNonce,
                    Log = "Got a bad value"
                });
            }  

            txCount += 1;

            Console.WriteLine("DeliverTx - txCount increment: ${txCount}");

            return Task.FromResult(new Tendermint.ABCI.Types.ResponseDeliverTx
            {
                Code = Tendermint.ABCI.Types.CodeType.Ok
            });
        }
        public override Task<Tendermint.ABCI.Types.ResponseCheckTx> CheckTx(Tendermint.ABCI.Types.RequestCheckTx request, Grpc.Core.ServerCallContext context)
        {
            var tx = request.Tx;

            if(tx.Length <= 4)
            {
                Int32 txCheck = Int32.Parse(tx.ToString());
                Console.WriteLine("CheckTx: " + txCheck);
                var msg = "Invalid nonce. Expected >= ${txCount}, got ${txCheck}";
                
                if(txCheck < txCount)
                {
                    Console.WriteLine("CheckTx ERROR: " + msg);

                    return Task.FromResult(new Tendermint.ABCI.Types.ResponseCheckTx
                    {
                        Code = Tendermint.ABCI.Types.CodeType.BadNonce,
                        Log = msg
                    });
                }
            }
            
            Console.WriteLine("CheckTx: OK");

            return Task.FromResult(new Tendermint.ABCI.Types.ResponseCheckTx{
                Code = Tendermint.ABCI.Types.CodeType.Ok
            });
        }
        
        public override Task<Tendermint.ABCI.Types.ResponseEcho> Echo(Tendermint.ABCI.Types.RequestEcho request, Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("Echo: " + request.Message);
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseEcho{ 
                Message = request.Message 
            });
        } 

        public override Task<Tendermint.ABCI.Types.ResponseFlush> Flush(Tendermint.ABCI.Types.RequestFlush request, Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("Flush: " + request.GetHashCode());
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseFlush());
        }

        public override Task<Tendermint.ABCI.Types.ResponseInfo> Info(Tendermint.ABCI.Types.RequestInfo request, Grpc.Core.ServerCallContext context)
        {
            var data = "[hashes: ${hashCOunt}, txs: ${txCount}]";
            Console.WriteLine("Info: " + data);
            
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseInfo{
                Data = data   
            });
        }

        public override Task<Tendermint.ABCI.Types.ResponseQuery> Query(Tendermint.ABCI.Types.RequestQuery request, Grpc.Core.ServerCallContext context)
        {
            //var data = request.Data.;

            Console.WriteLine("Query: " + request.GetHashCode());
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseQuery{
                Code = Tendermint.ABCI.Types.CodeType.Ok
            });
        }

        public override Task<Tendermint.ABCI.Types.ResponseSetOption> SetOption(Tendermint.ABCI.Types.RequestSetOption request, Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("SetOption: " + request.GetHashCode());
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseSetOption());
        }

        public override Task<Tendermint.ABCI.Types.ResponseBeginBlock> BeginBlock(Tendermint.ABCI.Types.RequestBeginBlock request, Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("BeginBlock: " + request.GetHashCode());
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseBeginBlock());

        }

        public override Task<Tendermint.ABCI.Types.ResponseEndBlock> EndBlock(Tendermint.ABCI.Types.RequestEndBlock request, Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("EndBlock: " + request.GetHashCode());
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseEndBlock());
        }

        public override Task<Tendermint.ABCI.Types.ResponseInitChain> InitChain(Tendermint.ABCI.Types.RequestInitChain request, Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("InitChain: " + request.GetHashCode());
            return Task.FromResult(new Tendermint.ABCI.Types.ResponseInitChain());

        }
    }
}
