using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public interface IServiceDefinitionService : IServiceDefinitionRepository
    {
        Task<IServiceSession> GetInstances(string serviceName);
    }
}
