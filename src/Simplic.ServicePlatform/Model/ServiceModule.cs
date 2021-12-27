using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ServiceModule
    {
        public string Name { get; set; }
        public IList<ServiceModuleConfiguration> Configuration { get; set; } = new List<ServiceModuleConfiguration>();
    }
}