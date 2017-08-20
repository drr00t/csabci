using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface IInfo
    {
        /**
         * Return information about the application state. Application specific.
         * 
         * @param req
         * @return
         */
        Task<ResponseInfo> RequestInfo(RequestInfo req);

    }
}