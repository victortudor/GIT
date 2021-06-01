using Microsoft.AspNetCore.Mvc;
using Rental.Business.Contracts;
using System;
using System.Threading.Tasks;

namespace Rental.API.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        readonly IRentalService rental;
        readonly ICurrencyExchangeService exchange;

        public VehicleController(IRentalService rentalService, ICurrencyExchangeService exchange)
        {
            rental = rentalService;
            this.exchange = exchange;
        }

        [Route("{customer}")]
        [HttpGet]
        public string GetAvailable(int customer)
        {
            var items = rental.GetAvailable(customer);
            return Newtonsoft.Json.JsonConvert.SerializeObject(items);
        }

        [Route("{customer}/{item}")]
        [HttpPost]
        public void Checkin(int customer, int item)
        {
            if (!rental.Checkin(item, customer)) Response.StatusCode = 500;
            else Response.StatusCode = 200;
        }

        [Route("{customer}/{item}/{damaged}/{filled}")]
        [HttpPost]
        public async Task<string> Checkout(int customer, int item, int damaged, int filled, [FromQuery] string currency)
        {
            if (damaged < 0 || filled < 0)
            {
                Response.StatusCode = 500;
                return "";
            }

            if (!rental.Checkout(item, customer, out DateTime? created))
            {
                Response.StatusCode = 500;
                return "";
            }
            else
            {
                if(!rental.GetItemRate(item, out uint rate))
                {
                    Response.StatusCode = 500;
                    return "";
                }

                var seconds = (DateTime.Now - created.Value).TotalSeconds;
                var pay = (float)((seconds * rate) + /* simulate some data from client */ damaged + filled);

                if (!string.IsNullOrWhiteSpace(currency))
                    pay = await exchange.Exchange(
                        pay,
                        new Business.Contracts.Models.Currency { Description = "EUR" }, // this can be identified from db of current item's configuration 
                        new Business.Contracts.Models.Currency { Description = currency }
                        );

                Response.StatusCode = 200;
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { Payment = pay });
            }
        }
    }
}
