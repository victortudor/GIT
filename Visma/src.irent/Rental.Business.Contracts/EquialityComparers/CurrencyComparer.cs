using Rental.Business.Contracts.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rental.Business.Contracts.EquialityComparers
{
    public sealed class CurrencyComparer : IEqualityComparer<Currency>
    {
        public bool Equals(Currency x, Currency y)
        {
            return x?.Id?.Equals(y.Id) ?? false;
        }

        public int GetHashCode([DisallowNull] Currency obj)
        {
            return obj.GetHashCode();
        }
    }
}
