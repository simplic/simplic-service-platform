using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public interface IServiceDefinitionService : IServiceDefinitionRepository
    {
        Task<ServiceInstance> GetInstances(string serviceName);
    }
}
