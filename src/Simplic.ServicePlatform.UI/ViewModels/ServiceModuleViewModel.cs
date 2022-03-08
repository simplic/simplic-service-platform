using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simplic.ServicePlatform.UI
{
    //Viewmodel Stufe 3
    public class ServiceModuleViewModel : ViewModelBase
    {
        private ServiceModule model;

        public ServiceModuleViewModel(ServiceModule model)
        {
            Model = new ServiceModule() { Name = model.Name, Configuration = new List<ServiceModuleConfiguration>(model.Configuration) };
            ConfigurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(Model.Configuration);
        }

        /// <summary>
        /// Synchronize all configurations from the binding with the model.
        /// </summary>
        public void Synch()
        {
            Model.Configuration = ConfigurationDefinitions.ToList();
        }

        public ServiceModule Model { get => model; set => model = value; }
        public ObservableCollection<ServiceModuleConfiguration> ConfigurationDefinitions { get; set; }
    }
}
