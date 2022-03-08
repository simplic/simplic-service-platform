using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// View model for the service definition.
    /// </summary>
    public class ServiceDefinitionViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields
        private ServiceDefinition model;
        private ServiceModule selectedServiceModule;
        private ServiceViewModel parent;
        private string serviceName;
        private DispatcherTimer timer;
        private ObservableCollection<ServiceModuleViewModel> usedModules;
        #endregion

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

        #region Private Methods

        private void Initialize()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
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
                Parent.FocusedElement = null;
            });
            UndoRenameCommand = new RelayCommand(o =>
            {
                ServiceName = Model.ServiceName;
                RaisePropertyChanged(nameof(ServiceName));
                Parent.FocusedElement = null;
            });

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RenameCommand.Execute(this);
            timer.Stop();
        }

        /// <summary>
        /// Overwrites target configuration values with native configuration values.
        /// </summary>
        /// <param name="nativeConfigurations">Used configuration values</param>
        /// <param name="targetConfigurations">Configurations that should be overwritten</param>
        private IEnumerable<ServiceModuleConfiguration> KeepConfigValues(IEnumerable<ServiceModuleConfiguration> nativeConfigurations, IEnumerable<ServiceModuleConfiguration> targetConfigurations)
        {
            var resultingConfigurations = new List<ServiceModuleConfiguration>(targetConfigurations);
            foreach (var resultingConfiguration in resultingConfigurations)
            {
                foreach (var nativeConfiguration in nativeConfigurations)
                {
                    if (nativeConfiguration.Name == null) continue;
                    if (nativeConfiguration.Name.Equals(resultingConfiguration.Name))
                        resultingConfiguration.Value = nativeConfiguration.Value;

                }
            }
            return resultingConfigurations;
        }

        #endregion

        #region Public Methods

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
            var newServiceModule = new ServiceModule()
            {
                Name = Parent.SelectedAvailableModule.Name,
                Configuration = new List<ServiceModuleConfiguration>
                    (
                        Parent.SelectedAvailableModule.ConfigurationDefinition.Select(config =>
                        {
                            return new ServiceModuleConfiguration() { Name = config.Name, Value = config.Default };
                        })
                    )
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


        // TODO There is a bug here which leads to the exact same instance of a module been given out.
        /// <summary>
        /// Updates the configurations of the given module with given ones.
        /// </summary>
        /// <param name="moduleName">Module</param>
        /// <param name="newConfigurations">New configurations</param>
        public void UpdateConfigurations(string moduleName, IEnumerable<ServiceModuleConfiguration> newConfigurations)
        {
            foreach (var module in UsedModules)
            {
                if (module.Model.Name.Equals(moduleName))
                {
                    var configurations = KeepConfigValues(module.ConfigurationDefinitions, newConfigurations);
                    module.ConfigurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(configurations);
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string ServiceName
        {
            get => serviceName;
            set
            {
                serviceName = value;
                RaisePropertyChanged(nameof(ServiceName));
                if (timer != null) timer.Start();
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
        public ServiceViewModel Parent { get => parent; set => parent = value; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public string Error { get => null; }

        /// <summary>
        /// Gets or sets the collection of errors.
        /// </summary>
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Checks a string for naming conventions.
        /// </summary>
        /// <param name="checkString">String to be checked</param>
        /// <returns>Error message</returns>
        public string this[string checkString]
        {
            get
            {
                string result = null;

                switch (checkString)
                {
                    case "ServiceName":
                        if (string.IsNullOrWhiteSpace(ServiceName))
                            result = "Service Name kann nicht leer sein";

                        else if (parent.Services.Where(x => x.ServiceName.ToLower() == ServiceName.ToLower()).Count() > 1)
                            result = "Service Name ist bereits vergeben";

                        break;
                        //parent.Services.FirstOrDefault(x => x.ServiceName == ServiceName) != null
                }
                if (ErrorCollection.ContainsKey(checkString))
                    ErrorCollection[checkString] = result;
                else if (result != null)
                {
                    ErrorCollection.Add(checkString, result);

                }

                RaisePropertyChanged("ErrorCollection");
                return result;
            }
        }
        #endregion
    }
}