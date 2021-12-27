using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ServiceDefinition
    {
        public string MachineName { get; set; }
        public string ServiceName { get; set; }
        public string Section { get; set; }
        public string Documentation { get; set; }
        public IList<ServiceModule> Modules { get; set; } = new List<ServiceModule>();
    }
}
