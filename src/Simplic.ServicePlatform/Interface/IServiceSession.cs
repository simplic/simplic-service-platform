using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public interface IServiceSession
    {
        IList<ServiceModuleInstance> Modules { get; set; }
    }
}
