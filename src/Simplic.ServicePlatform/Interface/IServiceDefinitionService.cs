using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Interface for managing service definitions
    /// </summary>
    public interface IServiceDefinitionService : IServiceDefinitionRepository
    {
        /// <summary>
        /// Create a new service instance by a service name.
        /// This method composes configurations and tries to load required modules.
        /// </summary>
        /// <param name="serviceName">Service name from /services/</param>
        /// <returns>Created / initialized service session instance</returns>
        Task<IServiceSession> GetInstances(string serviceName);
    }
}
