namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Represents a service-module configuration for a created service instance.
    /// </summary>
    public class ServiceModuleConfigurationInstance
    {
        /// <summary>
        /// Gets or sets the configuration name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration value. If no specific value was used this will contain the default value of the module-configuration.
        /// </summary>
        public string Value { get; set; }
    }
}