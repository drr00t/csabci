using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Examples
{
    public class CounterApp : ABCIApplicationClient
    {
        private int hashCount = 0;
        private int txCount = 0;

        public override Task<ResponseCheckTx> CheckTx(RequestCheckTx request, ServerCallContext context)
        {
            var tx = request.Tx;

            if (tx.Length <= 4)
            {
                Int32 txCheck = Int32.Parse(tx.ToString());
                // Console.WriteLine("CheckTx: " + txCheck);
                var msg = "Invalid nonce. Expected >= ${txCount}, got ${txCheck}";

                if (txCheck < txCount)
                {
                    Console.WriteLine("CheckTx ERROR: " + msg);

                    return Task.FromResult(new ResponseCheckTx
                    {
                        Code = CodeType.BadNonce,
                        Log = msg
                    });
                }
            }

            // Console.WriteLine("CheckTx: OK");

            return Task.FromResult(new ResponseCheckTx
            {
                Code = CodeType.Ok
            });
        }

        public override Task<ResponseDeliverTx> DeliverTx(RequestDeliverTx request, ServerCallContext context)
        {
            var tx = request.Tx;
            var data = tx.ToBase64();

            // Console.WriteLine("DeliverTx: ${data}");

            if (tx.Length == 0)
            {
                return Task.FromResult(new ResponseDeliverTx
                {
                    Code = CodeType.BadNonce,
                    Log = "Tansaction is emptyr"
                });
            }
            else if (tx.Length <= 4)
            {
                int x = Int32.Parse(tx.ToString());

                if (x != txCount)
                {
                    return Task.FromResult(new ResponseDeliverTx
                    {
                        Code = CodeType.BadNonce,
                        Log = "Invalid nonce"
                    });
                }
            }
            else
            {
                return Task.FromResult(new ResponseDeliverTx
                {
                    Code = CodeType.BadNonce,
                    Log = "Got a bad value"
                });
            }

            txCount += 1;

            // Console.WriteLine("DeliverTx - txCount increment: ${txCount}");

            return Task.FromResult(new ResponseDeliverTx
            {
                Code = CodeType.Ok
            });
        }


        public override Task<ResponseCommit> Commit(RequestCommit request, ServerCallContext context)
        {
            hashCount += 1;

            if (txCount == 0)
            {
                return Task.FromResult(new ResponseCommit
                {
                    Code = CodeType.Ok
                });
            }
            else
            {
                using (MemoryStream data = new MemoryStream())
                {
                    var bd = Encoding.UTF8.GetBytes(txCount.ToString());
                    data.Write(bd, 0, bd.Length - 1);

                    return Task.FromResult(new ResponseCommit
                    {
                        Code = CodeType.Ok,
                        Data = ByteString.FromStream(data)
                    });
                }
            }
        }

        public override Task<ResponseEcho> Echo(RequestEcho request, ServerCallContext context)
        {
            // Console.WriteLine("Echo: " + request.Message);
            return Task.FromResult(new ResponseEcho
            {
                Message = request.Message
            });
        }

        public override Task<ResponseInfo> Info(RequestInfo request, ServerCallContext context)
        {
            return Task.FromResult(new ResponseInfo
            {
                Data = String.Format("hashes:{0}, txs:{1}",hashCount, txCount)
            });
        }

        public override Task<ResponseQuery> Query(RequestQuery request, ServerCallContext context)
        {
            //var data = request.Data.;

            // Console.WriteLine("Query: " + request.Data);
            return Task.FromResult(new ResponseQuery
            {
                Code = CodeType.Ok
            });
        }

        public override Task<ResponseSetOption> SetOption(RequestSetOption request, ServerCallContext context)
        {
            // Console.WriteLine("SetOption: " + request.GetHashCode());
            return Task.FromResult(new ResponseSetOption());
        }

        public override Task<ResponseInitChain> InitChain(RequestInitChain request, ServerCallContext context)
        {
            // Console.WriteLine("Echo: " + request.Message);
            return Task.FromResult(new ResponseInitChain());
        }
    }
}
