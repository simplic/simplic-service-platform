namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Configuration name and value for a service definition (module)
    /// </summary>
    public class ServiceModuleConfiguration
    {
        /// <summary>
        /// Gets or sets the configuration name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration value, this will override the default value
        /// </summary>
        public string Value { get; set; }
    }
}