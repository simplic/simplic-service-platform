using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public interface IServiceDefinitionRepository
    {
        Task<IList<ServiceDefinition>> GetAll();
        Task Delete(string name);
        Task Save(ServiceDefinition service);
    }
}
