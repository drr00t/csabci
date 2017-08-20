using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface IEndBlock
    {

        //        * Signals the end of a block. Called prior to each Commit after all
        //    * transactions Returns:<br>
        //    * Validators ([]Validator): Changed validators with new voting powers (0 to
        //    * remove)
        //    * 
        //    * @param req the Request representing data about the EndBlock (height,...)
        //    */
        Task<ResponseEndBlock> RequestEndBlock(RequestEndBlock req);

    }
}