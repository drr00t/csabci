using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface ICheckTx
    {

        //Validate a transaction. This message should not mutate the state. Transactions are first run through CheckTx before broadcast to
        //peers in the mempool layer. You can make CheckTx semi-stateful and clear the state upon Commit or BeginBlock, to allow for dependent
        //sequences of transactions in the same block.
        //
        //@param req
        //@return

        Task<ResponseCheckTx> RequestCheckTx(RequestCheckTx req);
    }
}