using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Represents an instance of a module instance within a service.
    /// </summary>
    public class ServiceModuleInstance
    {
        /// <summary>
        /// Gets or sets the name of the module. See <see cref="ModuleDefinition.Name"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the module description. See <see cref="ModuleDefinition.Description"/>
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the module assembly. See <see cref="ModuleDefinition.Assembly"/>
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the composed module configuration.
        /// <para>This configuration will always contain all configuration values. Default values will be overriden by
        /// service-module configuration values.</para>
        /// </summary>
        public IList<ServiceModuleConfigurationInstance> Configuration { get; set; } = new List<ServiceModuleConfigurationInstance>();
    }
}
