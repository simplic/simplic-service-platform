namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Service for seaching all <see cref="ServiceModuleAttribute"/> and registering the related <see cref="IServiceModule"/>
    /// </summary>
    public interface IServiceModuleLookupService
    {
        /// <summary>
        /// Find and register all modules
        /// </summary>
        void RegisterServices();
    }
}
