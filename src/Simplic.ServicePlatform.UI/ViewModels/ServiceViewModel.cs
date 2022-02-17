using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Simplic.ServicePlatform.UI
{
    internal class ServiceViewModel : SSPViewModelBase
    {
        private ServiceDefinition serviceDefinition;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceDefinition">service</param>
        public ServiceViewModel(ServiceDefinition serviceDefinition)
        {
            this.serviceDefinition = serviceDefinition;
            SaveCommand = new RelayCommand(o => Save());
        }

        private void Save()
        {
            MessageBox.Show("not implemented");
        }

        /// <summary>
        /// Command for saving.
        /// </summary>
        public ICommand SaveCommand { get; set; }

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
