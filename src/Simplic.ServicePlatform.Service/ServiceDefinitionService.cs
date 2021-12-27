using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public class ServiceDefinitionService : IServiceDefinitionService
    {
        private readonly IServiceDefinitionRepository repository;
        private readonly IModuleDefinitionService moduleDefinitionService;

        public ServiceDefinitionService(IServiceDefinitionRepository repository, IModuleDefinitionService moduleDefinitionService)
        {
            this.repository = repository;
            this.moduleDefinitionService = moduleDefinitionService;
        }

        public Task Delete(string name) => repository.Delete(name);

        public Task<IList<ServiceDefinition>> GetAll() => repository.GetAll();

        public Task Save(ServiceDefinition service) => repository.Save(service);

        public async Task<IList<ServiceModuleInstance>> GetInstances(string serviceName, string machineName, string section = "default")
        {
            var result = new List<ServiceModuleInstance>();

            var modules = await moduleDefinitionService.GetAll();

            var services = (await GetAll())
                           .Where(x => x.MachineName?.ToLower() == machineName?.ToLower())
                           .Where(x => x.ServiceName?.ToLower() == serviceName?.ToLower())
                           .Where(x => x.Section?.ToLower() == section?.ToLower());

            foreach (var service in services)
            {
                foreach (var serviceModule in service.Modules)
                { 
                    // Find definition
                    var moduleDefinition = modules.FirstOrDefault(x => x.Name == serviceModule.Name);

                    if (moduleDefinition == null)
                        throw new Exception($"Could not find module definition: `{serviceModule.Name}`");

                    var instance = new ServiceModuleInstance
                    {
                        Name = serviceModule.Name,
                        Startup = moduleDefinition.Startup,
                        Description = moduleDefinition.Description,
                        Type = moduleDefinition.Type,
                        Worker = moduleDefinition.Worker
                    };

                    foreach (var configuation in serviceModule.Configuration)
                    {
                        var defaultValue = moduleDefinition.ConfigurationDefinition.FirstOrDefault(x => x.Name == configuation.Name)?.Default;

                        instance.Configuration.Add(new ServiceModuleConfigurationInstance 
                        {
                            Name = configuation.Name,
                            Value = string.IsNullOrWhiteSpace(configuation.Value) ? defaultValue : configuation.Value
                        });
                    }

                    foreach (var configuation in moduleDefinition.ConfigurationDefinition)
                    {
                        if (instance.Configuration.Any(x => x.Name == configuation.Name))
                            continue;

                        instance.Configuration.Add(new ServiceModuleConfigurationInstance
                        {
                            Name = configuation.Name,
                            Value = configuation.Default
                        });
                    }

                    result.Add(instance);
                }
            }

            return result;
        }
    }
}
