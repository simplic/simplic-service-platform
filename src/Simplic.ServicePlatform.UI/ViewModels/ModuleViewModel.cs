using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Simplic.UI.MVC;

namespace Simplic.ServicePlatform.UI
{
    public class ModuleViewModel : Simplic.UI.MVC.ViewModelBase
    {
        private IServiceClient serviceClient;
        private ModuleDefinition moduleDefinition;
        private ModuleDefinition selectedAvailableModule;
        private string selectedRequiredModule;
        private ObservableCollection<string> observableRequires;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="moduleDefinition">module</param>
        public ModuleViewModel(IServiceClient serviceClient, ModuleDefinition moduleDefinition)
        {
            this.serviceClient = serviceClient;
            this.moduleDefinition = moduleDefinition;
            LoadAvailableModules();
            ObservableRequires = new ObservableCollection<string>(Requires);

            SaveCommand = new RelayCommand(o => Save(), o => CanSave());
        }

        private void LoadAvailableModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                this.AvailableModules = new ObservableCollection<ModuleDefinition>(await serviceClient.GetAllModules());
            });
        }

        private void Save()
        {
            serviceClient.SaveModule(moduleDefinition);
        }

        //maybe add some regex
        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Assembly);
        }

        /// <summary>
        /// Parse module name to a more presentable format.
        /// </summary>
        /// <param name="name">Module name</param>
        /// <returns>Parsed name</returns>
        private string ParseModuleName(string name)
        {
            var nameBuilder = new StringBuilder(name);
            nameBuilder[0] -= (char)32;
            for (int i = 0; i < name.Length; i++)
                if (name[i] == '.')
                {
                    nameBuilder[i] = ' ';
                    if (i + 1 < name.Length)
                        nameBuilder[i + 1] = (char)(nameBuilder[i + 1] - 32);
                }

            return nameBuilder.ToString();
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
        public ModuleDefinition SelectedAvailableModule { get => selectedAvailableModule; set { selectedAvailableModule = value; RaisePropertyChanged(nameof(SelectedAvailableModule)); } }

        /// <summary>
        /// Gets or sets the selected available module.
        /// </summary>
        public string SelectedRequiredModule { get => selectedRequiredModule; set { selectedRequiredModule = value; RaisePropertyChanged(nameof(SelectedRequiredModule)); } }

        /// <summary>
        /// Gets a list of available modules.
        /// </summary>
        public ObservableCollection<ModuleDefinition> AvailableModules { get; set; }

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
        public IList<string> Requires { get => moduleDefinition.Requires; set { moduleDefinition.Requires = value; RaisePropertyChanged(nameof(Requires)); } }
        
        /// <summary>
        /// Gets or sets what the module requires.
        /// </summary>
        public ObservableCollection<string> ObservableRequires { get => observableRequires; set { observableRequires = value; RaisePropertyChanged(nameof(ObservableRequires)); } }

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
