using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Examples.CounterClient
{
    public class CounterClientApp : ABCIApplication.ABCIApplicationClient
    {
        private int hashCount = 0;
        private int txCount = 0;

        public override ResponseBeginBlock BeginBlock(RequestBeginBlock request, CallOptions options)
        {
            return base.BeginBlock(request, options);
        }

        public override ResponseCheckTx CheckTx(RequestCheckTx request, CallOptions options)
        {
            return base.CheckTx(request, options);
        }

        public override ResponseCommit Commit(RequestCommit request, CallOptions options)
        {
            return base.Commit(request, options);
        }

        public override ResponseDeliverTx DeliverTx(RequestDeliverTx request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.DeliverTx(request, headers, deadline, cancellationToken);
        }

        public override ResponseDeliverTx DeliverTx(RequestDeliverTx request, CallOptions options)
        {
            return base.DeliverTx(request, options);
        }

        public override ResponseEcho Echo(RequestEcho request, CallOptions options)
        {
            return base.Echo(request, options);
        }

        public override ResponseEndBlock EndBlock(RequestEndBlock request, CallOptions options)
        {
            return base.EndBlock(request, options);
        }

        public override ResponseFlush Flush(RequestFlush request, CallOptions options)
        {
            return base.Flush(request, options);
        }

        public override ResponseInfo Info(RequestInfo request, CallOptions options)
        {
            return base.Info(request, options);
        }

        public override ResponseInitChain InitChain(RequestInitChain request, CallOptions options)
        {
            return base.InitChain(request, options);
        }

        public override ResponseQuery Query(RequestQuery request, CallOptions options)
        {
            return base.Query(request, options);
        }

        public override ResponseSetOption SetOption(RequestSetOption request, CallOptions options)
        {
            return base.SetOption(request, options);
        }
    }
}
