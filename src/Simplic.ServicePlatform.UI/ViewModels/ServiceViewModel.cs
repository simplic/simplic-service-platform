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

    /// <summary>
    /// View model for the service.
    /// </summary>
    public class ServiceViewModel : ViewModelBase
    {
        #region Fields

        private readonly IServiceClient serviceClient;
        private ServiceDefinitionViewModel selectedServiceCard;
        private ModuleDefinition selectedAvailableModule;
        private ObservableCollection<ServiceDefinition> availableServiceDefinitions;
        private readonly List<ServiceDefinitionViewModel> servicesToRemove;
        private UIElement focusedElement;
        private string searchTerm;
        private readonly DispatcherTimer filterTimer;
        private int keyCounter;

        #endregion

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceClient">Service client</param>
        public ServiceViewModel(IServiceClient serviceClient)
        {

            this.serviceClient = serviceClient;
            Services = new ObservableCollection<ServiceDefinitionViewModel>();
            servicesToRemove = new List<ServiceDefinitionViewModel>();
            InitializeCommands();
            LoadServicesAndModules();
            filterTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.2) };
            filterTimer.Tick += Timer_Tick;
            keyCounter = 0;
        }


        #region Private Methods

        private void InitializeCommands()
        {
            AddCardCommand = new RelayCommand(AddCard);
            SaveCommand = new RelayCommand(Save);
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
                Services = new ObservableCollection<ServiceDefinitionViewModel>(availableServiceDefinitions.Select(m => new ServiceDefinitionViewModel(m, this)).OrderBy(x => x.Model.ServiceName));
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
                foreach (var service in Services)
                {
                    var newConfigurations = ModuleConfigurationConverter(availableModule.ConfigurationDefinition);
                    service.UpdateConfigurations(availableModule.Name, newConfigurations);
                }
            }
        }

        /// <summary>
        /// Converts configurations from module definitions to configurations for service modules.
        /// </summary>
        /// <param name="configurations">Configurations from a module definition</param>
        /// <returns>New configurations.</returns>
        private static IEnumerable<ServiceModuleConfiguration> ModuleConfigurationConverter(IEnumerable<ModuleConfigurationDefinition> configurations)
        {
            return configurations.Select(config => new ServiceModuleConfiguration {Name = config.Name, Value = config.Default}).ToList();
        }

        private void AddCard(object obj)
        {
            var newServiceCard = new ServiceDefinitionViewModel(new ServiceDefinition(), this);
            Services.Add(newServiceCard);
            SelectedServiceCard = newServiceCard;
        }

        private void DeleteCard(object obj)
        {
            Services.Remove(SelectedServiceCard);
            servicesToRemove.Add(new ServiceDefinitionViewModel(SelectedServiceCard.Model, this));
            RaisePropertyChanged(nameof(Services));
            SelectedServiceCard = null;
        }

        private void Save(object obj)
        {
            var errors = false;
            foreach (var service in Services)
            {
                service.Synch();
                if (service.HasErrors())
                {
                    errors = true;
                    continue; //this servers shouldnt be saved, go to next one
                }

                serviceClient.SaveService(service.Model);

                if (service.OldServiceName != null && !service.OldServiceName.Equals(service.Model.ServiceName))
                    servicesToRemove.Add(new ServiceDefinitionViewModel(new ServiceDefinition() { ServiceName = service.OldServiceName }, this));
            }

            foreach (var service in servicesToRemove)
            {
                serviceClient.DeleteService(service.Model);
            }

            if (errors) MessageBox.Show("Ein oder mehrere Services konnten nicht gespeichert werden.");
        }

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
            AvailableModulesCollectionView?.Refresh();

            return Task.CompletedTask;
        }

        private bool FilterModules(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            if (!(obj is ModuleDefinition moduleDefinition))
                return true;

            return moduleDefinition.Name.Contains(SearchTerm);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds service name to remove list.
        /// </summary>
        /// <param name="serviceName">Service name</param>
        public void RegisterServiceRemovalByName(string serviceName)
        {
            servicesToRemove.Add(new ServiceDefinitionViewModel(new ServiceDefinition { ServiceName = serviceName }, this));
        }

        #endregion

        #region Properties

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
        public UIElement FocusedElement
        {
            get => focusedElement;
            set
            {
                focusedElement = value;
                RaisePropertyChanged(nameof(FocusedElement));
            }
        }

        /// <summary>
        /// Gets or sets the command for saving.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the command for adding a card.
        /// </summary>
        public ICommand AddCardCommand { get; set; }

        /// <summary>
        /// Gets or sets the command for deleting a card.
        /// </summary>
        public ICommand DeleteCardCommand { get; set; }

        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
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

        #endregion
    }
}
