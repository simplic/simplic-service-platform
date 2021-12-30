using System;
using System.Runtime.Serialization;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Exception that will be thrown, if a service configuration is not existing in the repository under /servces/
    /// </summary>
    public class ServiceConfigurationNotFoundException : Exception
    {
        public ServiceConfigurationNotFoundException()
        {
        }

        public ServiceConfigurationNotFoundException(string message) : base(message)
        {
        }

        public ServiceConfigurationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceConfigurationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
