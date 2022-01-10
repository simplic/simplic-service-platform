using System.Collections.Generic;

namespace Simplic.ServicePlatform.Shared
{
    /// <summary>
    /// Represents a service-module for a service definition (<see cref="ServiceDefinition"/>
    /// </summary>
    public class ServiceModuleModel
    {
        /// <summary>
        /// Gets or sets the module name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a list of configuration-values
        /// </summary>
        public IList<ServiceModuleConfigurationModel> Configuration { get; set; } = new List<ServiceModuleConfigurationModel>();
    }
}