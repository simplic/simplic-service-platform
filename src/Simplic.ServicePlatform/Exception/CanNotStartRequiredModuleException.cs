using System;
using System.Runtime.Serialization;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Exception that will be thrown if a required module can't be started automatically.
    /// </summary>
    public class CanNotStartRequiredModuleException : Exception
    {
        public CanNotStartRequiredModuleException()
        {
        }

        public CanNotStartRequiredModuleException(string message) : base(message)
        {
        }

        public CanNotStartRequiredModuleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CanNotStartRequiredModuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
