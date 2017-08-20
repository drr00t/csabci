using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface IFlush
    {
        /**
 * Flush the response queue. Applications that implement types.Application need not implement this message -- it's handled by the project.
 * @param reqfl
 * @return
 */
        Task<ResponseFlush> RequestFlush(RequestFlush req);

    }
}