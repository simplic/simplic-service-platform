using System.Collections.Generic;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    public class ModuleViewModel : SSPViewModelBase
    {
        private ModuleDefinition moduleDefinition;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="moduleDefinition">module</param>
        public ModuleViewModel(ModuleDefinition moduleDefinition)
        {
            this.moduleDefinition = moduleDefinition;
        }

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
        public bool EnableAutoStart { get => moduleDefinition.EnableAutoStart; set => moduleDefinition.EnableAutoStart = value; }

        /// <summary>
        /// Gets or sets what the module requires.
        /// </summary>
        public IList<string> Requires { get => moduleDefinition.Requires; set => moduleDefinition.Requires = value; }

        /// <summary>
        /// Gets or sets the configuration for the module.
        /// </summary>
        public IList<ModuleConfigurationDefinition> ConfigurationDefinition { get => moduleDefinition.ConfigurationDefinition; set => moduleDefinition.ConfigurationDefinition = value;}

        /// <summary>
        /// Gets the name of the module in a more presentable format.
        /// </summary>
        public string ParsedName { get => ParseModuleName(moduleDefinition.Name); }

        /// <summary>
        /// Gets the string representing the status of EnableAutoStart.
        /// </summary>
        public string EnableAutoStartText { get { return moduleDefinition.EnableAutoStart ? "ON" : "OFF"; } }
    }
}
