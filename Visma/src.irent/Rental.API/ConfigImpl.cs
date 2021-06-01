using Rental.Business.Contracts;

namespace Rental.API
{

    /// <summary>
    /// Dummy implementation to implement <see cref="IConfig"/> for the DI container
    /// </summary>
    public class ConfigImpl : IConfig
    {
        public bool IsDebug => true;

        public bool Get<T>(string section, string property, out T value, bool @throw)
        {
            value = default;
            return false;
        }
    }
}
