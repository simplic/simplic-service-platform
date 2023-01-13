﻿using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Simplic.Studio.UI;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// View model for the service.
    /// </summary>
    public class ServiceViewModel : ViewModelBase
    {
        /// <summary>
        /// Used for the Collapse/Expand event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void CollapseEventHandler(object sender, CollapseEventArgs args);

        private readonly IServiceClient serviceClient;
        private ServiceDefinitionViewModel selectedServiceCard;
        private ModuleDefinition selectedAvailableModule;
        private ObservableCollection<ServiceDefinition> availableServiceDefinitions;
        private readonly List<ServiceDefinitionViewModel> servicesToRemove;
        private UIElement focusedElement;
        private string serviceSearchTerm;
        private string moduleSearchTerm;
        private readonly DispatcherTimer serviceFilterTimer;
        private readonly DispatcherTimer moduleFilterTimer;

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
            serviceFilterTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            serviceFilterTimer.Tick += ServiceFilterTimerTick;
            moduleFilterTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            moduleFilterTimer.Tick += ModuleFilterTimerTick;
        }

        private void InitializeCommands()
        {
            AddCardCommand = new RelayCommand(AddCard);
            SaveCommand = new RelayCommand(Save);
            DeleteCardCommand = new RelayCommand(DeleteCard, o => SelectedServiceCard != null);
            CollapseAllCommand = new RelayCommand(CollapseAll);
            ExpandAllCommand = new RelayCommand(ExpandAll);
        }

        private void LoadServicesAndModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                AvailableModules = new ObservableCollection<ModuleDefinition>((await serviceClient.GetAllModules()).OrderBy(x => x.Name));

                AvailableModulesCollectionView = CollectionViewSource.GetDefaultView(AvailableModules);
                AvailableModulesCollectionView.Filter = FilterModule;

                RaisePropertyChanged(nameof(AvailableModules));

                availableServiceDefinitions = new ObservableCollection<ServiceDefinition>(await serviceClient.GetAllServices());
                Services = new ObservableCollection<ServiceDefinitionViewModel>(availableServiceDefinitions.Select(m => new ServiceDefinitionViewModel(m, this)).OrderBy(x => x.Model.ServiceName));

                ServicesCollectionView = CollectionViewSource.GetDefaultView(Services);
                ServicesCollectionView.Filter = FilterService;

                UpdateServiceModules();
                RaisePropertyChanged(nameof(ServicesCollectionView));
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

        /// <summary>
        /// Adds a new service.
        /// </summary>
        /// <param name="obj"></param>
        private void AddCard(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var newServiceCard = new ServiceDefinitionViewModel(new ServiceDefinition(), this);
                Services.Add(newServiceCard);
                SelectedServiceCard = newServiceCard;
            });
        }

        /// <summary>
        /// Removes a service.
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteCard(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Services.Remove(SelectedServiceCard);
                servicesToRemove.Add(new ServiceDefinitionViewModel(SelectedServiceCard.Model, this));
                RaisePropertyChanged(nameof(Services));
                SelectedServiceCard = null;
            });
        }

        /// <summary>
        /// Collapses all visible service cards.
        /// </summary>
        /// <param name="obj"></param>
        private void CollapseAll(object obj)
        {
            foreach (ServiceDefinitionViewModel model in ServicesCollectionView)
            {
                CollapseEvent?.Invoke(this, new CollapseEventArgs(model));
            }
        }

        /// <summary>
        /// Expands all visible service cards.
        /// </summary>
        /// <param name="obj"></param>
        private void ExpandAll(object obj)
        {
            foreach (ServiceDefinitionViewModel model in ServicesCollectionView)
            {
                ExpandEvent?.Invoke(this, new CollapseEventArgs(model));
            }
        }

        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <param name="obj"></param>
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

        /// <summary>
        /// Removes services marked for removal.
        /// </summary>
        private void RemoveServices()
        {
            foreach (var service in servicesToRemove)
            {
                serviceClient.DeleteService(service.Model);
            }
        }

        private void ServiceFilterTimerTick(object sender, EventArgs e)
        {
            UpdateServicesView();
            serviceFilterTimer.Stop();
        }

        private void ModuleFilterTimerTick(object sender, EventArgs e)
        {
            UpdateAvailableModulesView();
            moduleFilterTimer.Stop();
        }

        /// <summary>
        /// Updates the filtered list of available modules.
        /// </summary>
        private void UpdateAvailableModulesView()
        {
            AvailableModulesCollectionView?.Refresh();
        }

        /// <summary>
        /// Updates the filtered list of services.
        /// </summary>
        private void UpdateServicesView()
        {
            ServicesCollectionView?.Refresh();
            RaisePropertyChanged(nameof(ServicesCollectionView));
        }

        /// <summary>
        /// Checks whether the service matches the search term.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FilterService(object obj)
        {
            if (string.IsNullOrWhiteSpace(ServiceSearchTerm))
                return true;

            if (!(obj is ServiceDefinitionViewModel serviceDefinition))
                return false;

            return serviceDefinition.ServiceName.ToLower().Contains(ServiceSearchTerm.ToLower());
        }

        /// <summary>
        /// Checks whether the module matches the search term.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FilterModule(object obj)
        {
            if (string.IsNullOrWhiteSpace(ModuleSearchTerm))
                return true;

            if (!(obj is ModuleDefinition moduleDefinition))
                return true;

            return moduleDefinition.Name.Contains(ModuleSearchTerm);
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
        /// Will be invoked when a card should be collapsed.
        /// </summary>
        public event CollapseEventHandler CollapseEvent;

        /// <summary>
        /// Will be invoked when a card should be expanded.
        /// </summary>
        public event CollapseEventHandler ExpandEvent;

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
        /// Gets or sets the command for collapsing all service cards.
        /// </summary>
        public ICommand CollapseAllCommand { get; set; }

        /// <summary>
        /// Gets or sets the command for expanding all service cards.
        /// </summary>
        public ICommand ExpandAllCommand { get; set; }

        /// <summary>
        /// Gets or sets the service search term.
        /// </summary>
        public string ServiceSearchTerm
        {
            get => serviceSearchTerm;
            set
            {
                serviceSearchTerm = value;
                RaisePropertyChanged(nameof(ServiceSearchTerm));
                serviceFilterTimer.Start();
            }
        }

        /// <summary>
        /// Gets or sets the module search term.
        /// </summary>
        public string ModuleSearchTerm
        {
            get => moduleSearchTerm;
            set
            {
                moduleSearchTerm = value;
                RaisePropertyChanged(nameof(ModuleSearchTerm));
                moduleFilterTimer.Start();
            }
        }

        /// <summary>
        /// Gets or sets the services collection view.
        /// </summary>
        public ICollectionView ServicesCollectionView { get; set; }

        /// <summary>
        /// Gets or sets the available modules collection view.
        /// </summary>
        public ICollectionView AvailableModulesCollectionView { get; set; }
    }
}
