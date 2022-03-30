using System.Collections.Generic;

namespace Simplic.ServicePlatform.Shared
{
    /// <summary>
    /// Represents a module definition
    /// </summary>
    public class ModuleDefinitionModel
    {
        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the module description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the assembly name, where the <see cref="IServiceModule"/> is registered using <see cref="ServiceModuleAttribute"/>.
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets whether auto-start for required modules is enabled.
        /// </summary>
        public bool EnableAutoStart { get; set; }

        /// <summary>
        /// Gets or sets a list of required modules. If <see cref="EnableAutoStart"/> is enabled for the required module,
        /// it will be started automatically.
        /// </summary>
        public IList<string> Requires { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a list of configuration values and the definition.
        /// </summary>
        public IList<ModuleConfigurationDefinitionModel> ConfigurationDefinition { get; set; } = new List<ModuleConfigurationDefinitionModel>();
    }
}
