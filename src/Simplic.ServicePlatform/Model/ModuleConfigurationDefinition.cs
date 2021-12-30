namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Represents a configuration-definition with a name and default value for a module definition.
    /// </summary>
    public class ModuleConfigurationDefinition
    {
        /// <summary>
        /// Gets or sets the configuration name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default value, that will be used if no override exists
        /// </summary>
        public string Default { get; set; }
    }
}