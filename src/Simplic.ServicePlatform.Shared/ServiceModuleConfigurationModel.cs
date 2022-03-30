namespace Simplic.ServicePlatform.Shared
{
    /// <summary>
    /// Configuration name and value for a service definition (module).
    /// </summary>
    public class ServiceModuleConfigurationModel
    {
        /// <summary>
        /// Gets or sets the configuration name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration value, this will override the default value.
        /// </summary>
        public string Value { get; set; }
    }
}