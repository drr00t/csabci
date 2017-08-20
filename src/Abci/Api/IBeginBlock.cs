using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface IBeginBlock
    {
        /**
            * Signals the beginning of a new block. Called prior to any DeliverTxs.
            * @param req
            * @return
            */
        Task<ResponseBeginBlock> RequestBeginBlock(RequestBeginBlock req);

    }
}