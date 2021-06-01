using System;

namespace Rental.Business.Contracts
{
    public interface IConfig
    {
        bool Get<T>(string section, string property, out T value, bool @throw);

        bool IsDebug { get; }
    }

    public static class IConfigurationExtensions
    {
        public static bool Get<T>(this IConfig configuration, Enum section, Enum property, out T value, bool @throw)
        {
            value = default;
            return configuration?.Get(section.ToString(), property.ToString(), out value, @throw) ?? @throw ? throw new ArgumentNullException($"{nameof(configuration)} is null") : false;
        }
    }
}
