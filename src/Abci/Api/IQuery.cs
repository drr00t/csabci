using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface IQuery
    {
        /**
         * Queries the Application for data
         */
        Task<ResponseQuery> RequestQuery(RequestQuery req);
    }
}