using Rental.Business.Contracts.Models;
using System.Threading.Tasks;

namespace Rental.Business.Contracts
{
    /// <summary>
    /// Just a abstraction over the middleware which deals with foreign currency
    /// </summary>
    public interface ICurrencyExchangeService
    {
        /// Exchange an ammount to desired <see cref="Currency"/>
        /// </summary>
        /// <param name="ammount">Sum to exchange</param>
        /// <param name="from">Before <see cref="Currency"/></param>
        /// <param name="to">After <see cref="Currency"/></param>
        /// <returns>Exchanged <paramref name="ammount"/></returns>
        Task<float> Exchange(float ammount, Currency from, Currency to);
    }
}
