using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public interface IServiceSession
    {
        string ServiceName { get; set; }
        string MachineName { get; set; }
        string Section { get; set; }
        IList<ServiceModuleInstance> Modules { get; }
    }
}
