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

        /// <summary>
        /// Gets or sets the service view model that has ben collapsed/expanded
        /// </summary>
        public ServiceDefinitionViewModel Service { get; set; }

    }
}
