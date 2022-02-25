using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.UI
{
    //Viewmodel Stufe 3
    public class ServiceModuleViewModel : ViewModelBase
    {
        private ObservableCollection<ModuleConfigurationDefinition> configurationDefinitions;
        private ServiceModule model;

        public ServiceModuleViewModel()
        {
            configurationDefinitions = new ObservableCollection<ModuleConfigurationDefinition>();
            configurationDefinitions.Add(new ModuleConfigurationDefinition() { Name = "Config1", Default = "Einstellung1"});
            configurationDefinitions.Add(new ModuleConfigurationDefinition() { Name = "Config2", Default = "Einstellung2"});
            configurationDefinitions.Add(new ModuleConfigurationDefinition() { Name = "Config3", Default = "Einstellung3"});
        }

        public ServiceModule Model { get => model; set => model = value; }
        public ObservableCollection<ModuleConfigurationDefinition> ConfigurationDefinitions { get => configurationDefinitions; set => configurationDefinitions = value; }
    }
}
