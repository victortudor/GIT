using System;
using System.Collections.Generic;

namespace Rental.Business.Contracts
{
    public interface IRentalService
    {
        IEnumerable<(int item, string itemName, int type, string typeName)> GetAvailable(int customer);
        bool Checkin(int item, int customer);
        bool Checkout(int item, int customer, out DateTime? created);
        bool GetCustomer(string name, string phone, out uint customer);
        bool GetItemRate(int item, out uint rate);
    }
}
