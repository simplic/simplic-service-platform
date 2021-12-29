using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ServiceDefinition
    {
        public string ServiceName { get; set; }
        public IList<ServiceModule> Modules { get; set; } = new List<ServiceModule>();
    }
}
