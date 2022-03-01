﻿using Simplic.UI.MVC;
using System.Collections.Generic;
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
        private ServiceDefinitionViewModel selectedServiceCard;
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
            LoadServicesAndModules();
            
            AddCardCommand = new RelayCommand(AddCard);
            SaveCommand = new RelayCommand(o => Save(), o => CanSave());
            DeleteCardCommand = new RelayCommand(DeleteCard);

        }


        private void LoadServicesAndModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                availableServiceDefinitions = new ObservableCollection<ServiceDefinition>(await serviceClient.GetAllServices());
                AvailableModules = new ObservableCollection<ModuleDefinition>(await serviceClient.GetAllModules());
            }).ContinueWith(o =>
            {
                Services = new ObservableCollection<ServiceDefinitionViewModel>(availableServiceDefinitions.Select(m => new ServiceDefinitionViewModel(m, this)));
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
            //var deleteCard = Services.FirstOrDefault(x => x.Model.ServiceName == selectedServiceCard.Model.ServiceName);
            //if(deleteCard == null)
            //{
            //    return;
            //}
            //Services.Remove(deleteCard);
            Services.Remove(SelectedServiceCard);
            RaisePropertyChanged(nameof(Services));
            SelectedServiceCard = null;
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
    }
}
