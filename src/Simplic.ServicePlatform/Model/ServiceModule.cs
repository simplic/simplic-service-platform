using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Represents a service-module for a service definition (<see cref="ServiceDefinition"/>
    /// </summary>
    public class ServiceModule
    {
        /// <summary>
        /// Gets or sets the module name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a list of configuration-values
        /// </summary>
        public IList<ServiceModuleConfiguration> Configuration { get; set; } = new List<ServiceModuleConfiguration>();
    }
}