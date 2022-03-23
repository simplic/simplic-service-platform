using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Simplic.Localization;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// View model for the service definition.
    /// </summary>
    public class ServiceDefinitionViewModel : ViewModelBase, IDataErrorInfo
    {
        private ServiceDefinition model;
        private ServiceModule selectedServiceModule;
        private string serviceName;
        private DispatcherTimer timer;
        private ObservableCollection<ServiceModuleViewModel> usedModules;

        /// <summary>
        /// Instantiates the view model for given model.
        /// </summary>
        public ServiceDefinitionViewModel(ServiceDefinition model, ServiceViewModel parent)
        {
            Model = model;
            Parent = parent;
            UsedModules = new ObservableCollection<ServiceModuleViewModel>();
            if (model.Modules != null)
                UsedModules = new ObservableCollection<ServiceModuleViewModel>(model.Modules.Select(m => new ServiceModuleViewModel(m)));

            ServiceName = model.ServiceName;
            OldServiceName = model.ServiceName;

            Initialize();
        }

        private void Initialize()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += Timer_Tick;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DropCommand = new RelayCommand(o => AddAvailableModule(), o => CanAddAvailableModule());

            RenameCommand = new RelayCommand(o =>
            {
                Model.ServiceName = ServiceName;
                RaisePropertyChanged(nameof(ServiceName));
            });

            UndoRenameCommand = new RelayCommand(o =>
            {
                ServiceName = Model.ServiceName;
                RaisePropertyChanged(nameof(ServiceName));
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RenameCommand.Execute(this);
            timer.Stop();
        }
        
        private string CheckServiceName()
        {
            var localization = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
            if (string.IsNullOrWhiteSpace(ServiceName))
                return localization.Translate("itemBoxProfile_display_name_empty");

            if (Parent.Services.Count(x => string.Equals(x.ServiceName, ServiceName, StringComparison.CurrentCultureIgnoreCase)) > 1)
                return localization.Translate("xaml_name_assigned");

            if (ServiceName.Length < 3)
                return localization.Translate("validation_name_at_least", "3");
            return string.Empty;
        }

        /// <summary>
        /// Overwrites target configuration values with native configuration values.
        /// </summary>
        /// <param name="nativeConfigurations">Used configuration values</param>
        /// <param name="targetConfigurations">Configurations that should be overwritten</param>
        private static IEnumerable<ServiceModuleConfiguration> CarryOverConfigValues(IEnumerable<ServiceModuleConfiguration> nativeConfigurations, IEnumerable<ServiceModuleConfiguration> targetConfigurations)
        {
            var resultingConfigurations = new List<ServiceModuleConfiguration>(targetConfigurations);
            foreach (var resultingConfiguration in resultingConfigurations)
            {
                foreach (var nativeConfiguration in nativeConfigurations.ToList().Where(nativeConfiguration => nativeConfiguration.Name == resultingConfiguration.Name))
                {
                    resultingConfiguration.Value = nativeConfiguration.Value;
                    break;
                }
            }
            return resultingConfigurations;
        }

        /// <summary>
        /// Adds a module to the service definition.
        /// </summary>
        /// <param name="serviceModule">Module</param>
        public void AddModule(ServiceModule serviceModule)
        {
            Model.Modules.Add(serviceModule);
            UsedModules.Add(new ServiceModuleViewModel(serviceModule));
            RaisePropertyChanged(nameof(Model));
            RaisePropertyChanged(nameof(UsedModules));
        }

        /// <summary>
        /// Adds the selected available module to this service's module collection.
        /// </summary>
        public void AddAvailableModule()
        {
            var newServiceModule = new ServiceModule
            {
                Name = Parent.SelectedAvailableModule.Name,
                Configuration = new List<ServiceModuleConfiguration>(Parent.SelectedAvailableModule.ConfigurationDefinition
                    .Select(config => new ServiceModuleConfiguration { Name = config.Name, Value = config.Default }))
            };
            AddModule(newServiceModule);
        }

        /// <summary>
        /// Checks whether the conditions are met for adding a module.
        /// </summary>
        public bool CanAddAvailableModule()
        {
            if (Parent.SelectedAvailableModule != null)
                return true;
            return false;
        }

        /// <summary>
        /// Update the view.
        /// </summary>
        public void Update()
        {
            RaisePropertyChanged(nameof(Model));
            RaisePropertyChanged(nameof(UsedModules));
        }

        /// <summary>
        /// Synchronizes the bindings with the model.
        /// </summary>
        public void Synch()
        {
            foreach (var module in UsedModules)
                module.Synch();
            Model.Modules = new List<ServiceModule>(UsedModules.Select(m => m.Model));
        }

        /// <summary>
        /// Updates the configurations of the given module with given ones.
        /// </summary>
        /// <param name="moduleName">Module</param>
        /// <param name="newConfigurations">New configurations</param>
        public void UpdateConfigurations(string moduleName, IEnumerable<ServiceModuleConfiguration> newConfigurations)
        {
            foreach (var module in UsedModules)
            {
                if (!module.Model.Name.Equals(moduleName)) continue;
                var configurations = CarryOverConfigValues(module.ConfigurationDefinitions, newConfigurations);
                module.ConfigurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(configurations);
            }
        }

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string ServiceName
        {
            get => serviceName;
            set
            {
                if (!string.IsNullOrEmpty(this[value])) return;
                serviceName = value;
                RaisePropertyChanged(nameof(ServiceName));
                timer?.Start();
            }
        }

        /// <summary>
        /// Gets or sets the old service name.
        /// Equal to service name if service name is not changed.
        /// </summary>
        public string OldServiceName { get; set; }

        /// <summary>
        /// Gets or sets the collection of used modules.
        /// </summary>
        public ObservableCollection<ServiceModuleViewModel> UsedModules
        {
            get => usedModules;
            set
            {
                usedModules = value;
                RaisePropertyChanged(nameof(UsedModules));
            }
        }

        /// <summary>
        /// Gets or sets the model for the service definition.
        /// </summary>
        public ServiceDefinition Model
        {
            get => model;
            set
            {
                model = value;
                RaisePropertyChanged(nameof(Model));
            }
        }

        /// <summary>
        /// Gets or sets the selected module.
        /// </summary>
        public ServiceModule SelectedServiceModule
        {
            get => selectedServiceModule;
            set
            {
                selectedServiceModule = value;
                RaisePropertyChanged(nameof(SelectedServiceModule));
            }
        }

        /// <summary>
        /// Gets or sets the command for handling drop events.
        /// </summary>
        public ICommand DropCommand { get; set; }

        /// <summary>
        /// Gets or sets the rename command.
        /// </summary>
        public ICommand RenameCommand { get; set; }

        /// <summary>
        /// Gets or sets the undo rename command.
        /// </summary>
        public ICommand UndoRenameCommand { get; set; }

        /// <summary>
        /// Gets and sets the parent of this viewmodel.
        /// </summary>
        public new ServiceViewModel Parent { get; set; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public string Error => null;

        /// <summary>
        /// Gets or sets the collection of errors.
        /// </summary>
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Returns whether this service has errors.
        /// </summary>
        public bool HasErrors()
        {
            return !string.IsNullOrEmpty(this[nameof(ServiceName)]);
        }

        /// <summary>
        /// Checks a string for naming conventions.
        /// </summary>
        /// <param name="checkString">String that contains the subject to be checked (e.g. "ServiceName")</param>
        /// <returns>Error message</returns>
        public string this[string checkString]
        {
            get
            {
                string result = null;
                switch (checkString)
                {
                    case nameof(ServiceName):
                        result = CheckServiceName();
                        break;
                }
                if (string.IsNullOrEmpty(result))
                    return result;

                ErrorCollection[checkString] = result;
                RaisePropertyChanged(nameof(ErrorCollection));
                return result;
            }
        }
    }
}