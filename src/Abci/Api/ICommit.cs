using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface ICommit
    {

        /***
         * Return a Merkle root hash of the application state.
         */
        Task<ResponseCommit> RequestCommit(RequestCommit req);

    }
}