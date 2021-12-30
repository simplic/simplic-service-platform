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
        private readonly IServiceSession serviceSession;

        public ServiceDefinitionService(IServiceDefinitionRepository repository, IModuleDefinitionService moduleDefinitionService, IServiceSession serviceSession)
        {
            this.repository = repository;
            this.moduleDefinitionService = moduleDefinitionService;
            this.serviceSession = serviceSession;
        }

        public Task Delete(string name) => repository.Delete(name);

        public Task<IList<ServiceDefinition>> GetAll() => repository.GetAll();
        public Task<ServiceDefinition> Get(string serviceName) => repository.Get(serviceName);

        public Task Save(ServiceDefinition service) => repository.Save(service);

        public async Task<IServiceSession> GetInstances(string serviceName)
        {
            serviceSession.Modules.Clear();
            var modules = await moduleDefinitionService.GetAll();
            var service = await Get(serviceName);

            var services = service.Modules.ToList();

            // Load required services
            foreach (var serviceModule in services.ToList())
            {
                var moduleDefinition = modules.FirstOrDefault(x => x.Name == serviceModule.Name);

                if (moduleDefinition == null || moduleDefinition.Requires == null || !moduleDefinition.Requires.Any())
                    continue;

                foreach (var requiredModule in moduleDefinition.Requires)
                {
                    // Module already loaded
                    if (services.Any(x => x.Name == requiredModule))
                        continue;

                    var requiredModuleDefinition = modules.FirstOrDefault(x => x.Name == requiredModule);

                    if (!requiredModuleDefinition.EnableAutoStart)
                        throw new Exception($"The required module can not be loaded automatically, because EnableAutoStart is not enabled: `{requiredModule}`");

                    services.Add(new ServiceModule 
                    {
                        Name = requiredModuleDefinition.Name
                    });
                }
            }


            // TODO: Exception handling

            foreach (var serviceModule in services)
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

                var requiredServices = new List<string>();

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

                serviceSession.Modules.Add(instance);
            }

            return serviceSession;
        }
    }
}
