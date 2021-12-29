using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public class ServiceModuleInstance
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Assembly { get; set; }
        public IList<ServiceModuleConfigurationInstance> Configuration { get; set; } = new List<ServiceModuleConfigurationInstance>();
    }
}
