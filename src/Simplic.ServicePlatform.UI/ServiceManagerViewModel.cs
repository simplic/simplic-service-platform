using System.Collections.Generic;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    internal class ServiceManagerViewModel : ServiceManagerViewModelBase
    {
        private IList<ModuleDefinition> moduleDefinitions;
        private IModuleDefinitionService moduleDefinitionService;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="moduleDefinitionService"></param>
        public ServiceManagerViewModel(IModuleDefinitionService moduleDefinitionService)
        {
            this.moduleDefinitionService = moduleDefinitionService;
            Application.Current.Dispatcher.Invoke(async () =>
            {
                moduleDefinitions = await moduleDefinitionService.GetAll() as List<ModuleDefinition>;
            });
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Assembly { get; set; }
        public bool EnableAutoStart { get; set; }

    }
}
