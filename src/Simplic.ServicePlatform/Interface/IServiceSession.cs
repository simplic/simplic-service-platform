using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public interface IServiceSession
    {
        IList<ServiceModuleInstance> Modules { get; set; }
    }
}
