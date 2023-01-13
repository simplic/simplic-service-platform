using System;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Arguments for the service card collapse/expand event
    /// </summary>
    public class CollapseEventArgs : EventArgs
    {
        public CollapseEventArgs(ServiceDefinitionViewModel service)
        {
            Service = service;
        }

        public ServiceDefinitionViewModel Service { get; set; }

    }
}
