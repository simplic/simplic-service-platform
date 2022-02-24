using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simplic.ServicePlatform.UI
{
    public class ServiceDefinitionViewModel : ViewModelBase
    {
        private ServiceDefinition model;
        
        public ServiceDefinitionViewModel()
        {
        }

        public void AddModule(ServiceModule serviceModule)
        {
            UsedModules.Add(serviceModule);
            Model.Modules.Add(serviceModule);
        }

        public void Update()
        {
            RaisePropertyChanged(nameof(Model));
            RaisePropertyChanged(nameof(UsedModules));
        }

        public ObservableCollection<ServiceModule> UsedModules { get; set; }
        public ServiceDefinition Model { get => model; set => model = value; }
    }
}