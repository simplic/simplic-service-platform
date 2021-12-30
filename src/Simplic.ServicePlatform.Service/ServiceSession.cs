using System.Collections.Generic;

namespace Simplic.ServicePlatform.Service
{
    public class ServiceSession : IServiceSession
    {
        public string MachineName { get; set; }
        public string ServiceName { get; set; }
        public string Section { get; set; }
        public IList<ServiceModuleInstance> Modules { get; } = new List<ServiceModuleInstance>();
    }
}
