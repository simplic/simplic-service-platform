using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Represents a runnign service session.
    /// </summary>
    public interface IServiceSession
    {
        /// <summary>
        /// Gets or sets the actual service name
        /// </summary>
        string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the machine name that the service is running on
        /// </summary>
        string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the actual used ini-section
        /// </summary>
        string Section { get; set; }

        /// <summary>
        /// Gets all loaded and initialized modules
        /// </summary>
        IList<ServiceModuleInstance> Modules { get; }
    }
}
