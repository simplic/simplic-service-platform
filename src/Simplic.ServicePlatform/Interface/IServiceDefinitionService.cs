using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public interface IServiceDefinitionService : IServiceDefinitionRepository
    {
        Task<IList<ServiceModuleInstance>> GetInstances(string serviceName, string machineName, string section = "default");
    }
}
