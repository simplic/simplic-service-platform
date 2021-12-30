using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Repository for reading and writing service definitions.
    /// </summary>
    public interface IServiceDefinitionRepository
    {
        /// <summary>
        /// Get all service definitions from /services/
        /// </summary>
        /// <returns>List of service definitions</returns>
        Task<IList<ServiceDefinition>> GetAll();

        /// <summary>
        /// Gets a specific service-definition by its name
        /// </summary>
        /// <param name="name">Service name /services/`name`.json</param>
        /// <returns>Service instance if found, else null</returns>
        Task<ServiceDefinition> Get(string name);

        /// <summary>
        /// Delete service instance by its name
        /// </summary>
        /// <param name="name">Service name</param>
        Task Delete(string name);

        /// <summary>
        /// Create or update service definition in the database
        /// </summary>
        /// <param name="service">Service definition to save</param>
        Task Save(ServiceDefinition service);
    }
}
