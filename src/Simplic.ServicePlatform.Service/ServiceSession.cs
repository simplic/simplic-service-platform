using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ServiceSession : IServiceSession
    {
        public string MachineName { get; set; }
        public string ServiecName { get; set; }
        public string Section { get; set; }
        public IList<ServiceModuleInstance> Modules { get; set; } = new List<ServiceModuleInstance>();
    }
}
