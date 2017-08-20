using System.Threading.Tasks;
using Tendermint.Abci.Types;


namespace Tendermint.Abci.Api
{
    public interface IEcho
    {
        /**
         * Undocumented
         * @param req
         * @return
         */
        Task<ResponseEcho> RequestEcho(RequestEcho req);
    }
}