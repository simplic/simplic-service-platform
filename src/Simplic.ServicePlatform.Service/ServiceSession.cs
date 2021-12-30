using System.Collections.Generic;

namespace Simplic.ServicePlatform.Service
{
    /// <inheritdoc/>
    public class ServiceSession : IServiceSession
    {
        /// <inheritdoc/>
        public string MachineName { get; set; }

        /// <inheritdoc/>
        public string ServiceName { get; set; }

        /// <inheritdoc/>
        public string Section { get; set; }

        /// <inheritdoc/>
        public IList<ServiceModuleInstance> Modules { get; } = new List<ServiceModuleInstance>();
    }
}
