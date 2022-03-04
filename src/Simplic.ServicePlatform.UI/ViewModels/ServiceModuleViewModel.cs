using Simplic.UI.MVC;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Simplic.ServicePlatform.UI
{
    //Viewmodel Stufe 3
    public class ServiceModuleViewModel : ViewModelBase
    {
        private ServiceModule model;

        public ServiceModuleViewModel(ServiceModule model)
        {
            Model = model;
            ConfigurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(Model.Configuration);
            UpdateValuesCommand = new RelayCommand(O =>
            {
                Model.Configuration = ConfigurationDefinitions;
                RaisePropertyChanged(nameof(ConfigurationDefinitions));
            });
            UndoUpdateValuesCommand = new RelayCommand(O =>
            {
                for (int i = 0; i < ConfigurationDefinitions.Count && i < Model.Configuration.Count; i++)
                    ConfigurationDefinitions[i] = Model.Configuration[i];
                RaisePropertyChanged(nameof(ConfigurationDefinitions));
            });

        }

        /// <summary>
        /// Synchronize all configurations from the binding with the model.
        /// </summary>
        public void Synch()
        {
            Model.Configuration = ConfigurationDefinitions;
        }

        public ServiceModule Model { get => model; set => model = value; }
        public ObservableCollection<ServiceModuleConfiguration> ConfigurationDefinitions { get; set; }
        public ICommand UpdateValuesCommand { get; set; }
        public ICommand UndoUpdateValuesCommand { get; set; }
    }
}
