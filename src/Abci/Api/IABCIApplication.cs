
namespace Tendermint.Abci.Api
{
    public interface IABCIApplication : IDeliverTx, IBeginBlock, ICheckTx, ICommit, IEndBlock, IFlush, IInfo, IInitChain, IQuery, ISetOption, IEcho
    {

    }
}