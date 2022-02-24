using Simplic.UI.MVC;
using System.Collections.ObjectModel;

namespace Simplic.ServicePlatform.UI
{
    public class ServiceDefinitionViewModel : ViewModelBase
    {
        private ServiceDefinition model;
        public ObservableCollection<ServiceModule> AvailableModules { get; set; } = new ObservableCollection<ServiceModule>();
        public ServiceDefinition Model { get => model; set => model = value; }
    }
}