using System;
using System.Threading.Tasks;
using Tendermint.Abci.Api;
using Tendermint.Abci.Types;

namespace Tendermint.Abci
{
    public class ABCIApplicationBase : IABCIApplication
    {
        // ILogger logger = loggerFactory.CreateLogger<Program>();

        private int hashCount = 0;
        private int txCount = 0;

        public virtual Task<ResponseDeliverTx> ReceivedDeliverTx(RequestDeliverTx req)
        {
            var tx = req.Tx;
            var data = tx.ToBase64();

            Console.WriteLine("DeliverTx: ${data}");

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

            Console.WriteLine("DeliverTx - txCount increment: ${txCount}");

            return Task.FromResult(new ResponseDeliverTx
            {
                Code = CodeType.Ok
            });
        }
        public virtual Task<ResponseCheckTx> RequestCheckTx(RequestCheckTx req)
        {
            var tx = req.Tx;

            if (tx.Length <= 4)
            {
                Int32 txCheck = Int32.Parse(tx.ToString());
                Console.WriteLine("CheckTx: " + txCheck);
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

            Console.WriteLine("CheckTx: OK");

            return Task.FromResult(new ResponseCheckTx
            {
                Code = CodeType.Ok
            });
        }


        public virtual Task<ResponseCommit> RequestCommit(RequestCommit req)
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
                using (System.IO.MemoryStream data = new System.IO.MemoryStream())
                {
                    var bd = System.Text.Encoding.UTF8.GetBytes(txCount.ToString());
                    data.Write(bd, 0, bd.Length - 1);

                    return Task.FromResult(new ResponseCommit
                    {
                        Code = CodeType.Ok,
                        Data = Google.Protobuf.ByteString.FromStream(data)
                    });
                }
            }
        }


        public virtual Task<ResponseInfo> RequestInfo(RequestInfo req)
        {
            var data = "[hashes: ${hashCOunt}, txs: ${txCount}]";
            Console.WriteLine("Info: " + data);

            return Task.FromResult(new ResponseInfo
            {
                Data = data
            });
        }

        public virtual Task<ResponseQuery> RequestQuery(RequestQuery req)
        {
            //var data = request.Data.;

            Console.WriteLine("Query: " + req.GetHashCode());
            return Task.FromResult(new ResponseQuery
            {
                Code = CodeType.Ok
            });
        }

        public virtual Task<ResponseSetOption> RequestSetOption(RequestSetOption req)
        {
            Console.WriteLine("SetOption: " + req.GetHashCode());
            return Task.FromResult(new ResponseSetOption());
        }


        public virtual Task<ResponseEcho> RequestEcho(RequestEcho req)
        {
            Console.WriteLine("Echo: " + req.Message);
            return Task.FromResult(new ResponseEcho
            {
                Message = req.Message
            });
        }

        public virtual Task<ResponseFlush> RequestFlush(RequestFlush req)
        {
            Console.WriteLine("Flush: " + req.GetHashCode());
            return Task.FromResult(new ResponseFlush());
        }

        public virtual Task<ResponseBeginBlock> RequestBeginBlock(RequestBeginBlock req)
        {
            Console.WriteLine("BeginBlock: " + req.GetHashCode());
            return Task.FromResult(new ResponseBeginBlock());

        }

        public virtual Task<ResponseEndBlock> RequestEndBlock(RequestEndBlock req)
        {
            Console.WriteLine("EndBlock: " + req.GetHashCode());
            return Task.FromResult(new ResponseEndBlock());
        }

        public virtual Task<ResponseInitChain> RequestInitChain(RequestInitChain req)
        {
            Console.WriteLine("InitChain: " + req.GetHashCode());
            return Task.FromResult(new ResponseInitChain());
        }
    }
}

