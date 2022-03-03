using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Manages HTTP requests for gathering ServiceDefinitions and ModuleDefinitions.
    /// </summary>
    public interface IServiceClient
    {
        /// <summary>
        /// Gets all services.
        /// </summary>
        /// <returns>Collection of service definitions.</returns>
        Task<IEnumerable<ServiceDefinition>> GetAllServices();

        /// <summary>
        /// Gets a particular service provided by name.
        /// </summary>
        /// <param name="name">Service name</param>
        /// <returns>Service definition</returns>
        Task<ServiceDefinition> GetService(string name);

        /// <summary>
        /// Gets all modules.
        /// </summary>
        /// <returns>Collection of module definitions.</returns>
        Task<IEnumerable<ModuleDefinition>> GetAllModules();

        /// <summary>
        /// Gets a particular module provided by name.
        /// </summary>
        /// <param name="name">Module name</param>
        /// <returns>Module definition</returns>
        Task<ModuleDefinition> GetModule(string name);

        /// <summary>
        /// Saves the given service definition,
        /// </summary>
        /// <param name="service">Service</param>
        /// <returns></returns>
        Task SaveService(ServiceDefinition service);

        /// <summary>
        /// Saves the given module definition.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns></returns>
        Task SaveModule(ModuleDefinition module);

        /// <summary>
        /// Deletes the given service.
        /// </summary>
        /// <param name="service">Service</param>
        /// <returns></returns>
        Task DeleteService(ServiceDefinition service);
    }
}
