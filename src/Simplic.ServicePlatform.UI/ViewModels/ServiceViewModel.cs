using Simplic.ServicePlatform.UI.ViewModels;
using Simplic.ServicePlatform.UI.Views;
using Simplic.UI.MVC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Simplic.ServicePlatform.UI
{

    //ViewModel Stufe 1
    public class ServiceViewModel : Simplic.UI.MVC.ViewModelBase
    {
        private readonly IServiceClient serviceClient;
        private ModuleDefinition selectedAvailableModule;
        private ObservableCollection<ServiceDefinition> availableServiceDefinitions;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceDefinition">service</param>
        public ServiceViewModel(IServiceClient serviceClient)
        {

            this.serviceClient = serviceClient;
            Services = new ObservableCollection<ServiceDefinitionViewModel>();
            LoadAvailableServices();
            LoadAvailableModules();
            AddCardCommand = new RelayCommand(AddNewCard);
            SaveCommand = new RelayCommand(o => Save(), o => CanSave());
        }

        public void AddNewCard(object obj)
        {
            //var window = new AddCardView();
            //window.ShowDialog();

            Services.Add(new ServiceDefinitionViewModel(new ServiceDefinition { Id = new System.Guid() }, this));
            RaisePropertyChanged(nameof(Services));
        }

        private void LoadAvailableServices()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                availableServiceDefinitions = new ObservableCollection<ServiceDefinition>(await serviceClient.GetAllServices());
            }).ContinueWith(o =>
            {
                //AvailableServices = new ObservableCollection<ServiceDefinitionViewModel>(
                //    serviceDefinitions.Select(m =>
                //    new ServiceDefinitionViewModel() { Model = m, UsedModules = new ObservableCollection<ServiceModule>(m.Modules) })
                //);
                Services = new ObservableCollection<ServiceDefinitionViewModel>(availableServiceDefinitions.Select(m => new ServiceDefinitionViewModel(m, this)));
                RaisePropertyChanged(nameof(Services));
            });
        }

        private void LoadAvailableModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                AvailableModules = new ObservableCollection<ModuleDefinition>(await serviceClient.GetAllModules());
                RaisePropertyChanged(nameof(AvailableModules));
            });
        }
        private void Save()
        {
            MessageBox.Show("Noooo dont do it!");
            //foreach (var service in Services)
            //    serviceClient.SaveService(service.Model);
        }

        /// <summary>
        /// Returns whether it should be possible to save.
        /// </summary>
        /// <returns>True or False</returns>
        private bool CanSave()
        {
            foreach (var service in Services)
                if (!string.IsNullOrEmpty(service.Model.ServiceName))
                    return false;
            return true;
        }

        /// <summary>
        /// Gets or sets the selected available module.
        /// </summary>
        public ModuleDefinition SelectedAvailableModule
        {
            get => selectedAvailableModule;
            set
            {
                selectedAvailableModule = value;
                RaisePropertyChanged(nameof(SelectedAvailableModule));
            }
        }

        /// <summary>
        /// Gets a list of available modules.
        /// </summary>
        public ObservableCollection<ModuleDefinition> AvailableModules { get; set; }

        /// <summary>
        /// Gets or sets services.
        /// </summary>
        public ObservableCollection<ServiceDefinitionViewModel> Services { get; set; }

        /// <summary>
        /// Gets or sets the command for saving.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the command for adding cards.
        /// </summary>
        public ICommand AddCardCommand { get; set; }

    }
}
