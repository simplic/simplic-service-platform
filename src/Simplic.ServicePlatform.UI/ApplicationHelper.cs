using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.UI
{
    public class ApplicationHelper
    {
        IModuleDefinitionService moduleDefinitionService;

        public void ShowWindow(UInt16 choice)
        {
            switch (choice)
            {
                case 0:
                    var serviceDummy = new ServiceDefinition
                    {
                        Id = new Guid("12345678-abcd-abab-cdcd-abababababab"),
                        ServiceName = "Example Service",
                        Modules = new List<ServiceModule> {
                        new ServiceModule {
                            Name = "SomeServiceModule",
                            Configuration = new List<ServiceModuleConfiguration>
                            {
                                new ServiceModuleConfiguration{ Name = "Some Configuration", Value = "Some Value" },
                                new ServiceModuleConfiguration{ Name = "Some Other Configuration", Value = "Some Other Value" },
                                new ServiceModuleConfiguration{ Name = "Some Other Other Configuration", Value = "Some Other Other Value" }
                            }
                        }
                    }
                    };
                    ShowServiceView(moduleDefinitionService, serviceDummy);
                    break;

                case 1:
                    var moduleDummy = new ModuleDefinition
                    {
                        Name = "simplic.does.not.exist",
                        Description = "This plugin doesn't exist",
                        Assembly = "simplic.plugin.some.assembly",
                        EnableAutoStart = true,
                        Requires = new List<string> { },
                        ConfigurationDefinition = new List<ModuleConfigurationDefinition>()
                    {
                        new ModuleConfigurationDefinition { Default = "default1", Name = "someName" },
                        new ModuleConfigurationDefinition { Default = "default2", Name = "someOtherName" }
                    }
                    };
                    ShowModuleView(moduleDefinitionService, moduleDummy);
                    break;

                default:
                    return;
            }



        }

        private void ShowServiceView(IModuleDefinitionService moduleDefinitionService, ServiceDefinition serviceDefinition)
        {
            var view = new ServiceView(moduleDefinitionService, serviceDefinition);
            view.Show();
        }

        private void ShowModuleView(IModuleDefinitionService moduleDefinitionService, ModuleDefinition moduleDefinition)
        {
            var view = new ModuleView(moduleDefinitionService, moduleDefinition);
            view.Show();
        }
    }
}
