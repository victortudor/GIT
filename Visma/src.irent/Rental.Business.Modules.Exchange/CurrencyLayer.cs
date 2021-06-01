using Rental.Business.Contracts;
using Rental.Business.Contracts.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rental.Business.Modules.Exchange
{
    /// <summary>
    /// Implementation of <see cref="ICurrencyExchangeService"/> over https://currencylayer.com/ API
    /// </summary>
    public sealed class CurrencyLayer : ICurrencyExchangeService
    {
        /// <summary>
        /// HTTP client factory 
        /// </summary>
        readonly IHttpClientFactory http;

        /// <summary>
        /// Runtime configuration
        /// </summary>
        readonly IConfig configuration;

        public CurrencyLayer(IHttpClientFactory http, IConfig configuration)
        {
            this.http = http;
            this.configuration = configuration;
        }

        public async Task<float> Exchange(float ammount, Currency from, Currency to)
        {
            var local = (float)-1;
            if (!configuration.Get(nameof(ICurrencyExchangeService), "api_key", out string api_key, false))
                return -1;

            if (!configuration.Get(nameof(ICurrencyExchangeService), "exchange_endpoint", out string exchange_endpoint, false))
                return -1;

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{exchange_endpoint}?from={from.Id}&to={to.Id}&amount={ammount}&access_key={api_key}"))
            {
                using var client = http.CreateClient();
                using var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return local;

                // {"success":false,"error":{"code":105,"info":"Access Restricted - Your current Subscription Plan does not support this API Function."}}
            }

            return local;
        }
    }
}
