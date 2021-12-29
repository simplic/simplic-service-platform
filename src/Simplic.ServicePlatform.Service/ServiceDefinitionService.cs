using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.Service
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
        public Task<ServiceDefinition> Get(string serviceName) => repository.Get(serviceName);

        public Task Save(ServiceDefinition service) => repository.Save(service);

        public async Task<ServiceInstance> GetInstances(string serviceName)
        {
            var result = new ServiceInstance();

            var modules = await moduleDefinitionService.GetAll();
            var service = await Get(serviceName);

            // TODO: Exception handling

            foreach (var serviceModule in service.Modules)
            {
                // Find definition
                var moduleDefinition = modules.FirstOrDefault(x => x.Name == serviceModule.Name);

                if (moduleDefinition == null)
                    throw new Exception($"Could not find module definition: `{serviceModule.Name}`");

                var instance = new ServiceModuleInstance
                {
                    Name = serviceModule.Name,
                    Description = moduleDefinition.Description,
                    Assembly = moduleDefinition.Assembly
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

                result.Modules.Add(instance);
            }

            return result;
        }
    }
}
