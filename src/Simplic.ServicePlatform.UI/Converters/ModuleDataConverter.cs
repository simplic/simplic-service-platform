using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.DragDrop.Behaviors;

namespace Simplic.ServicePlatform.UI
{
    public class ModuleDataConverter : DataConverter
    {
        /// <summary>
        /// Singleton object for static access.
        /// </summary>
        public static ModuleDataConverter Singleton = new ModuleDataConverter();
        
        /// <summary>
        /// Converts configurations from module definitions to configurations for service modules.
        /// </summary>
        /// <param name="configurations">Configurations from a module definition</param>
        /// <returns>New configurations.</returns>
        public IList<ServiceModuleConfiguration> ModuleConfigurationConverter(IEnumerable<ModuleConfigurationDefinition> configurations)
        {
            var newConfig = new List<ServiceModuleConfiguration>();
            foreach (var config in configurations)
                newConfig.Add(new ServiceModuleConfiguration() { Name = config.Name, Value = config.Default });
            return newConfig;
        }

        /// <summary>
        /// Converts a module definition to a service module.
        /// </summary>
        /// <param name="moduleDefinition"></param>
        /// <returns></returns>
        public ServiceModule ModuleDefinitionConverter(ModuleDefinition moduleDefinition)
        {
            return new ServiceModule() { Name = moduleDefinition.Name, Configuration = ModuleConfigurationConverter(moduleDefinition.ConfigurationDefinition) };
        }

        /// <summary>
        /// Converts a module definition to a service module view model.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <returns>Service module if correct data and format correct, null otherwise.</returns>
        public override object ConvertTo(object data, string format)
        {
            var moduleDefinition = DataObjectHelper.GetData(data, typeof(ModuleDefinition), false) as IEnumerable<object>;
            if (moduleDefinition != null && format == typeof(ServiceModuleViewModel).FullName)
            {
                return new ServiceModuleViewModel(ModuleDefinitionConverter(moduleDefinition.FirstOrDefault() as ModuleDefinition));
            }
            return null;
        }

        /// <summary>
        /// Gets the ConvertTo formats.
        /// </summary>
        /// <returns>Formats</returns>
        public override string[] GetConvertToFormats()
        {
            return new string[] { typeof(ModuleDefinition).FullName, typeof(ServiceModule).FullName };
        }
    }
}
