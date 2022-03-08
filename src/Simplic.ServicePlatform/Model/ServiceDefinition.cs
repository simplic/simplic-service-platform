using System.Collections.Generic;
using System;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Represents the definition/configuration of an application service (service)
    /// </summary>
    public class ServiceDefinition
    {
        /// <summary>
        /// Gets or sets the service id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the service name
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets all modules in this service
        /// </summary>
        public IList<ServiceModule> Modules { get; set; } = new List<ServiceModule>();
    }
}
