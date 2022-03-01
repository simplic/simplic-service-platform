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
        /// Converts configurations from module definitions to configurations for service modules.
        /// </summary>
        /// <param name="configurations">Configurations from a module definition</param>
        /// <returns>New configurations.</returns>
        private IList<ServiceModuleConfiguration> ModuleConfigurationConverter(IEnumerable<ModuleConfigurationDefinition> configurations)
        {
            var newConfig = new List<ServiceModuleConfiguration>();
            foreach (var config in configurations)
                newConfig.Add(new ServiceModuleConfiguration { Name = config.Name, Value = config.Default });
            return newConfig;
        }

        /// <summary>
        /// Converts a module definition to a service module.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <returns>Service module if correct data and format correct, null otherwise.</returns>
        public override object ConvertTo(object data, string format)
        {
            var moduleDefinition = (ModuleDefinition)DataObjectHelper.GetData(data, typeof(ModuleDefinition), false);
            if (moduleDefinition == null && format == typeof(ServiceModuleViewModel).FullName)
            {
                var newServiceModule = new ServiceModule { Name = moduleDefinition.Name, Configuration = ModuleConfigurationConverter(moduleDefinition.ConfigurationDefinition) };
                return new ServiceModuleViewModel(newServiceModule);
            }

            return null;
        }

        public override string[] GetConvertToFormats()
        {
            return new string[] { typeof(ModuleDefinition).FullName, typeof(ServiceModule).FullName };
        }
    }
}
