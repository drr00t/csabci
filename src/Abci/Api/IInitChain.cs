using System.Threading.Tasks;
using Tendermint.Abci.Types;

namespace Tendermint.Abci.Api
{
    public interface IInitChain
    {
        /**
         * Called once upon genesis <br>
         * Arguments:<br>
         * 
         * Validators ([]Validator): Initial genesis validators
         * 
         * @param req
         */
        Task<ResponseInitChain> RequestInitChain(RequestInitChain req);

    }
}