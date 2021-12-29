using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ServiceInstance
    {
        public IList<ServiceModuleInstance> Modules { get; set; } = new List<ServiceModuleInstance>();
    }
}
