using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface ISetOption
    {

        /**
         * Set application options. E.g. Key="mode", Value="mempool" for a mempool connection, or Key="mode", Value="consensus" for a consensus connection. Other options are application specific.
         * @param req
         * @return
         */
        Task<ResponseSetOption> RequestSetOption(RequestSetOption req);
    }
}