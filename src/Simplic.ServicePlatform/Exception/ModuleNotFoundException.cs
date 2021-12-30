using System;
using System.Runtime.Serialization;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Will be thrown if a module definition that was used in a service is not existing.
    /// </summary>
    public class ModuleNotFoundException : Exception
    {
        public ModuleNotFoundException()
        {
        }

        public ModuleNotFoundException(string message) : base(message)
        {
        }

        public ModuleNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Module { get; set; }
    }
}
