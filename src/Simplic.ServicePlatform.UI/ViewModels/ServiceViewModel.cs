using System;
using System.Collections.Generic;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    internal class ServiceViewModel : SSPViewModelBase
    {
        private ServiceDefinition serviceDefinition;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceDefinition">module</param>
        public ServiceViewModel(ServiceDefinition serviceDefinition)
        {
            this.serviceDefinition = serviceDefinition;
        }

        /// <summary>
        /// Gets or sets the id of the service.
        /// </summary>
        public Guid Id { get => serviceDefinition.Id; set => serviceDefinition.Id = value; }

        /// <summary>
        /// Gets or sets the service name of the service.
        /// </summary>
        public string ServiceName { get => serviceDefinition.ServiceName; set => serviceDefinition.ServiceName = value; }

        /// <summary>
        /// Gets or sets the modules of the service.
        /// </summary>
        public IList<ServiceModule> Modules { get => serviceDefinition.Modules; set => serviceDefinition.Modules = value; }
    }
}
