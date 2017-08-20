using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface IDeliverTx
    {
        /**
         * Append and run a transaction. If the transaction is valid, returns CodeType.OK
         * @param req
         * @return
         */
        Task<ResponseDeliverTx> ReceivedDeliverTx(RequestDeliverTx req);
    }
}