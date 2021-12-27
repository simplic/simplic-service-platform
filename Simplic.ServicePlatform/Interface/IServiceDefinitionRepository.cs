using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
