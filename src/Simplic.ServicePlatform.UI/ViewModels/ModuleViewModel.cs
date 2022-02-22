using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Simplic.UI.MVC;

namespace Simplic.ServicePlatform.UI
{
    public class ModuleViewModel : SSPViewModelBase
    {
        private IModuleDefinitionService moduleDefinitionService;
        private ModuleDefinition moduleDefinition;
        private ModuleDefinition selectedAvailableModule;
        private string selectedRequiredModule;
        private ObservableCollection<string> observableRequires;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="moduleDefinition">module</param>
        public ModuleViewModel(IModuleDefinitionService moduleDefinitionService, ModuleDefinition moduleDefinition)
        {
            this.moduleDefinitionService = moduleDefinitionService;
            this.moduleDefinition = moduleDefinition;
            LoadAvailableModules();
            ObservableRequires = new ObservableCollection<string>(Requires);

            SaveCommand = new RelayCommand(o => Save(), o => CanSave());
        }

        private void LoadAvailableModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                this.AvailableModules = await moduleDefinitionService.GetAll();
            });
        }

        private void Save()
        {
            moduleDefinitionService.Save(moduleDefinition);
        }

        //maybe add some regex
        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Assembly);
        }

        public void UpdateRequires()
        {
            Requires = ObservableRequires;
        }

        /// <summary>
        /// Command for saving.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the selected available module.
        /// </summary>
        public ModuleDefinition SelectedAvailableModule { get => selectedAvailableModule; set { selectedAvailableModule = value; OnPropertyChanged(); } }

        /// <summary>
        /// Gets or sets the selected available module.
        /// </summary>
        public string SelectedRequiredModule { get => selectedRequiredModule; set { selectedRequiredModule = value; OnPropertyChanged(); } }

        /// <summary>
        /// Gets a list of available modules.
        /// </summary>
        public IList<ModuleDefinition> AvailableModules { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        public string Name { get => moduleDefinition.Name; set => moduleDefinition.Name = value; }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get => moduleDefinition.Description; set => moduleDefinition.Description = value; }

        /// <summary>
        /// Gets or sets the assembly of the module.
        /// </summary>
        public string Assembly { get => moduleDefinition.Assembly; set => moduleDefinition.Assembly = value; }

        /// <summary>
        /// Gets or sets whether autostart should be enabled for the module.
        /// </summary>
        public bool EnableAutoStart { get => moduleDefinition.EnableAutoStart; set { moduleDefinition.EnableAutoStart = value; } }

        /// <summary>
        /// Gets or sets what the module requires.
        /// </summary>
        public IList<string> Requires { get => moduleDefinition.Requires; set { moduleDefinition.Requires = value; OnPropertyChanged(); } }
        
        /// <summary>
        /// Gets or sets what the module requires.
        /// </summary>
        public ObservableCollection<string> ObservableRequires { get => observableRequires; set { observableRequires = value; OnPropertyChanged(); } }

        /// <summary>
        /// Gets or sets the configuration for the module.
        /// </summary>
        public IList<ModuleConfigurationDefinition> ConfigurationDefinition { get => moduleDefinition.ConfigurationDefinition; set => moduleDefinition.ConfigurationDefinition = value; }

        /// <summary>
        /// Gets the name of the module in a more presentable format.
        /// </summary>
        public string ParsedName { get => ParseModuleName(moduleDefinition.Name); }
    }
}
