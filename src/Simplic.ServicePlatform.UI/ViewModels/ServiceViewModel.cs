using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Simplic.ServicePlatform.UI
{

    //ViewModel Stufe 1
    public class ServiceViewModel : Simplic.UI.MVC.ViewModelBase
    {
        private readonly IServiceClient serviceClient;
        private ServiceDefinitionViewModel selectedServiceCard;
        private ModuleDefinition selectedAvailableModule;
        private ObservableCollection<ServiceDefinition> availableServiceDefinitions;
        private List<ServiceDefinitionViewModel> servicesToRemove;
        private UIElement focusedElement;
        private string searchTerm;
        private DispatcherTimer filterTimer;
        private int keyCounter;


        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceDefinition">service</param>
        public ServiceViewModel(IServiceClient serviceClient)
        {

            this.serviceClient = serviceClient;
            Services = new ObservableCollection<ServiceDefinitionViewModel>();
            servicesToRemove = new List<ServiceDefinitionViewModel>();
            InitializeCommands();
            LoadServicesAndModules();
            filterTimer = new DispatcherTimer();
            filterTimer.Interval = TimeSpan.FromSeconds(0.2);
            filterTimer.Tick += Timer_Tick;
            keyCounter = 0;
        }

        private void InitializeCommands()
        {
            AddCardCommand = new RelayCommand(AddCard);
            SaveCommand = new RelayCommand(o => Save(), o => CanSave());
            //DeleteCommand = new RelayCommand(Delete);
            DeleteCardCommand = new RelayCommand(DeleteCard, o => SelectedServiceCard != null);
        }

        private void LoadServicesAndModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                availableServiceDefinitions = new ObservableCollection<ServiceDefinition>(await serviceClient.GetAllServices());
                AvailableModules = new ObservableCollection<ModuleDefinition>((await serviceClient.GetAllModules()).OrderBy(x => x.Name));

                AvailableModulesCollectionView = CollectionViewSource.GetDefaultView(AvailableModules);
                AvailableModulesCollectionView.Filter = FilterModules;
            }).ContinueWith(o =>
            {
                Services = new ObservableCollection<ServiceDefinitionViewModel>((availableServiceDefinitions.Select(m => new ServiceDefinitionViewModel(m, this))).OrderBy(x => x.Model.ServiceName));
                UpdateServiceModules();
                RaisePropertyChanged(nameof(Services));
                RaisePropertyChanged(nameof(AvailableModules));
            });
        }

        /// <summary>
        /// Updates all service modules accordingly to module definitions.
        /// </summary>
        private void UpdateServiceModules()
        {
            foreach (var availableModule in AvailableModules)
            {
                var newConfigurations = ModuleConfigurationConverter(availableModule.ConfigurationDefinition);
                foreach (var service in Services)
                {
                    service.UpdateConfigurations(availableModule.Name, newConfigurations);
                }
            }
        }

        /// <summary>
        /// Converts configurations from module definitions to configurations for service modules.
        /// </summary>
        /// <param name="configurations">Configurations from a module definition</param>
        /// <returns>New configurations.</returns>
        private IEnumerable<ServiceModuleConfiguration> ModuleConfigurationConverter(IEnumerable<ModuleConfigurationDefinition> configurations)
        {
            var newConfig = new List<ServiceModuleConfiguration>();
            foreach (var config in configurations)
                newConfig.Add(new ServiceModuleConfiguration { Name = config.Name, Value = config.Default });
            return newConfig;
        }

        private void AddCard(object obj)
        {
            Services.Add(new ServiceDefinitionViewModel(new ServiceDefinition(), this));
        }

        private void DeleteCard(object obj)
        {
            Services.Remove(SelectedServiceCard);
            servicesToRemove.Add(new ServiceDefinitionViewModel(SelectedServiceCard.Model, this));
            RaisePropertyChanged(nameof(Services));
            SelectedServiceCard = null;
        }

        private void Save()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Services.FirstOrDefault().Model); //just for debugging purposes
            foreach (var service in Services)
            {
                service.Synch();
                serviceClient.SaveService(service.Model);

                if (!service.OldServiceName.Equals(service.Model.ServiceName))
                    servicesToRemove.Add(new ServiceDefinitionViewModel { Model = new ServiceDefinition { ServiceName = service.OldServiceName } });
            }

            foreach (var service in servicesToRemove)
            {
                serviceClient.DeleteService(service.Model);
            }
        }

        //private void Delete()
        //{
        //    foreach (var service in Services)
        //    {
        //        //serviceClient.DeleteService(service.Model);
        //    }
        //}

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
        /// Event handler for timer tick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_Tick(object sender, EventArgs e)
        {
            keyCounter++;

            if (keyCounter >= 2)
            {
                await UpdateAvailableModulesView();
                keyCounter = 0;
                filterTimer.Stop();
            }
        }

        private Task UpdateAvailableModulesView()
        {
            if (AvailableModulesCollectionView != null)
                AvailableModulesCollectionView.Refresh();

            return Task.CompletedTask;
        }

        public bool FilterModules(object obj)
        {
            if (!(obj is ModuleDefinition))
                return true;

            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            var moduleDefinition = obj as ModuleDefinition;

            if (moduleDefinition.Name.Contains(SearchTerm))
                return true;

            return false;
        }

        /// <summary>
        /// Adds service name to remove list.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        public void RegisterServiceRemovalByName(string serviceName)
        {
            servicesToRemove.Add(new ServiceDefinitionViewModel(new ServiceDefinition { ServiceName = serviceName }, this));
        }

        /// <summary>
        /// Gets or sets the selected service card.
        /// </summary>
        public ServiceDefinitionViewModel SelectedServiceCard
        {
            get => selectedServiceCard;
            set
            {
                selectedServiceCard = value;
                RaisePropertyChanged(nameof(SelectedServiceCard));
            }
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
        /// Gets or sets the focused element.
        /// </summary>
        public UIElement FocusedElement { get => focusedElement; set { focusedElement = value; RaisePropertyChanged(nameof(FocusedElement)); } }

        /// <summary>
        /// Gets or sets the command for saving.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the commadn for adding a card.
        /// </summary>
        public ICommand AddCardCommand { get; set; }

        /// <summary>
        /// Gets or sets the command for deleting a card.
        /// </summary>
        public ICommand DeleteCardCommand { get; set; }

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                searchTerm = value;
                RaisePropertyChanged(nameof(SearchTerm));
                filterTimer.Start();
            }
        }

        /// <summary>
        /// Gets or sets the available modules collection view
        /// </summary>
        public ICollectionView AvailableModulesCollectionView { get; set; }
    }
}
