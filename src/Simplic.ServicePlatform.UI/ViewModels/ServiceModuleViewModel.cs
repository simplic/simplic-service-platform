using Simplic.UI.MVC;
using System.Collections.ObjectModel;

namespace Simplic.ServicePlatform.UI
{
    //Viewmodel Stufe 3
    public class ServiceModuleViewModel : ViewModelBase
    {
        private ObservableCollection<ServiceModuleConfiguration> configurationDefinitions;
        private ServiceModule model;

        public ServiceModuleViewModel(ServiceModule model)
        {
            Model = model;
            configurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(model.Configuration);
        }

        /// <summary>
        /// Synchronize all configurations from the binding with the model.
        /// </summary>
        public void Synch()
        {
            Model.Configuration = ConfigurationDefinitions;
        }

        public ServiceModule Model { get => model; set => model = value; }
        public ObservableCollection<ServiceModuleConfiguration> ConfigurationDefinitions { get => configurationDefinitions; set => configurationDefinitions = value; }
    }
}
