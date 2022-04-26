using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Simplic.Log;
using Simplic.PlugIn.Monitoring.Data;
using Simplic.PlugIn.Monitoring.Service;
using Simplic.Studio.UI;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// View model for the service.
    /// </summary>
    public class ServiceViewModel : ViewModelBase
    {
        private readonly IServiceClient serviceClient;
        private readonly LogStorageService logStorageService;
        private ServiceDefinitionViewModel selectedServiceCard;
        private ModuleDefinition selectedAvailableModule;
        private ObservableCollection<ServiceDefinition> availableServiceDefinitions;
        private readonly List<ServiceDefinitionViewModel> servicesToRemove;
        private UIElement focusedElement;
        private string searchTerm;
        private readonly DispatcherTimer filterTimer;
        private int keyCounter;
        private string commandString;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceClient">Service client</param>
        public ServiceViewModel(IServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
            logStorageService = new LogStorageService(new LogMongoDBRepository("localhost:27017", "service_logs"));
            Services = new ObservableCollection<ServiceDefinitionViewModel>();
            servicesToRemove = new List<ServiceDefinitionViewModel>();
            InitializeCommands();
            LoadServicesAndModules();
            filterTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.2) };
            filterTimer.Tick += Timer_Tick;
            keyCounter = 0;
            ConsoleStartDate = DateTime.Now.AddDays(-7);
            ConsoleEndDate = DateTime.Now;

            ShowLogLevel = new Dictionary<string, bool>
            {
                {"Trace", false},
                {"Debug", true},
                {"Information", true},
                {"Warning", true},
                {"Error", true},
                {"Critical", true}
            };
        }

        private void InitializeCommands()
        {
            AddCardCommand = new RelayCommand(AddCard);
            SaveCommand = new RelayCommand(Save);
            DeleteCardCommand = new RelayCommand(DeleteCard, o => SelectedServiceCard != null);
            ExecuteCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrWhiteSpace(CommandString) || SelectedServiceLog == null) return;
                if (!SelectedServiceLog.Any())
                    MessageBox.Show("Unable to retrieve endpoint: Service has not send any logs yet.");
                var client = new UdpClient();
                var data = Encoding.UTF8.GetBytes(CommandString);
                client.Send(data, data.Length, RetrieveEndPoint(SelectedServiceLog));
                client.Close();
                CommandString = string.Empty;
            });
            InitializeServiceLogCommand = new RelayCommand(o =>
            {
                if (SelectedServiceCard == null || SelectedServiceCard.Model == null) return;

                SelectedServiceLog = RetrieveServiceLogMessages(SelectedServiceCard.Model);
                RaisePropertyChanged(nameof(SelectedServiceLog));
            });
            RefreshServiceLogCommand = new RelayCommand(o =>
            {
                if (SelectedServiceCard == null || SelectedServiceCard.Model == null) return;

                foreach (var missingLog in RetrieveMissingServiceLogMessages(SelectedServiceCard.Model, SelectedServiceLog))
                    SelectedServiceLog = SelectedServiceLog.Append(missingLog);
                RaisePropertyChanged(nameof(SelectedServiceLog));
            });
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
            return configurations.Select(config => new ServiceModuleConfiguration { Name = config.Name, Value = config.Default }).ToList();
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
                    continue;
                }

                serviceClient.SaveService(service.Model);

                CheckRenameAndRegisterRemoval(service);
            }

            RemoveServices();

            if (errors) LocalizedMessageBox.Show("error_save_services", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckRenameAndRegisterRemoval(ServiceDefinitionViewModel service)
        {
            if (service.OldServiceName != null && !service.OldServiceName.Equals(service.Model.ServiceName))
                servicesToRemove.Add(new ServiceDefinitionViewModel(new ServiceDefinition { ServiceName = service.OldServiceName }, this));
        }

        private void RemoveServices()
        {
            foreach (var service in servicesToRemove)
            {
                serviceClient.DeleteService(service.Model);
            }
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

        private static IPEndPoint RetrieveEndPoint(IEnumerable<ServiceLogMessage> logMessages)
        {
            var orderedLogMessages = logMessages.OrderBy(x => x.Time);
            var lastLogMessage = orderedLogMessages.Last();
            var thisExternalIp = new WebClient().DownloadString("https://api.ipify.org");
            var targetIp = lastLogMessage.Ip == thisExternalIp ? "127.0.0.1" : lastLogMessage.Ip;
            return new IPEndPoint(IPAddress.Parse(targetIp), lastLogMessage.Port);
        }

        private IEnumerable<ServiceLogMessage> RetrieveServiceLogMessages(ServiceDefinition serviceDefinition)
        {
            if (serviceDefinition == null || string.IsNullOrWhiteSpace(serviceDefinition.ServiceName))
                return Enumerable.Empty<ServiceLogMessage>();
            var table = $"Simplic {serviceDefinition.ServiceName}".Replace(" ", "_").ToLower();
            return logStorageService.Read(table).Where(IsFiltered).OrderBy(x => x.Time); ;
        }

        private IEnumerable<ServiceLogMessage> RetrieveMissingServiceLogMessages(ServiceDefinition serviceDefinition, IEnumerable<ServiceLogMessage> serviceLog)
        {
            if (serviceDefinition == null || string.IsNullOrWhiteSpace(serviceDefinition.ServiceName) || serviceLog == null)
                return Enumerable.Empty<ServiceLogMessage>();
            var table = $"Simplic {serviceDefinition.ServiceName}".Replace(" ", "_").ToLower();

            var lastLog = serviceLog.Last();
            return logStorageService.Read(table, $"{nameof(lastLog.UnixTimestamp)} > {lastLog.UnixTimestamp}")
                                    .Where(IsFiltered).OrderBy(x => x.Time); // TODO: only read messages that have a unix timestamp > last log message

        }

        private bool IsFiltered(ServiceLogMessage logMessage)
        {
            var inRange = logMessage.Time <= ConsoleEndDate && logMessage.Time >= ConsoleStartDate;
            return inRange && ShowLogLevel[logMessage.LogLevel.ToString()];
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
                InitializeServiceLogCommand.Execute(this);
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
        /// Gets or sets the command for executing a command.
        /// </summary>
        public ICommand ExecuteCommand { get; set; }

        /// <summary>
        /// Gets or sets the command for initializing the console.
        /// </summary>
        public ICommand InitializeServiceLogCommand { get; set; }

        /// <summary>
        /// Gets or sets the command for refreshing the service log.
        /// </summary>
        public ICommand RefreshServiceLogCommand { get; set; }


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

        /// <summary>
        /// Gets or sets the service log xaml.
        /// </summary>
        public IEnumerable<ServiceLogMessage> SelectedServiceLog { get; set; }

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        public string CommandString
        {
            get => commandString;
            set
            {
                commandString = value;
                RaisePropertyChanged(nameof(CommandString));
            }
        }

        /// <summary>
        /// Gets or sets the console start date.
        /// </summary>
        public DateTime ConsoleStartDate { get; set; }

        /// <summary>
        /// Gets or sets the console end date.
        /// </summary>
        public DateTime ConsoleEndDate { get; set; }

        /// <summary>
        /// Gets or sets the filter for log levels.
        /// </summary>
        public Dictionary<string, bool> ShowLogLevel { get; set; }
    }
}
