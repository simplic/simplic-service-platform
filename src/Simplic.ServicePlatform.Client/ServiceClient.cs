using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simplic.Configuration;
using Simplic.WebApi.Client;

namespace Simplic.ServicePlatform.Client
{
    /// <summary>
    /// Manages HTTP requests for gathering ServiceDefinitions and ModuleDefinitions.
    /// </summary>
    public class ServiceClient : ClientBase, IServiceClient
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionConfigurationService">Service for connection configurations</param>
        public ServiceClient(IConnectionConfigurationService connectionConfigurationService)
            : base(connectionConfigurationService.GetByName("SimplicWebApi").ConnectionString)
        {

        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ModuleDefinition>> GetAllModules()
        {
            var json = await base.GetAsync<string>(Url + $"api/{ApiVersion}", "ModuleDefinition", "GetAll", null);
            return JsonConvert.DeserializeObject<IEnumerable<ModuleDefinition>>(json);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ServiceDefinition>> GetAllServices()
        {
            var json = await base.GetAsync<string>(Url + $"api/{ApiVersion}", "ServiceDefinition", "GetAll", null);
            return JsonConvert.DeserializeObject<IEnumerable<ServiceDefinition>>(json);
        }

        /// <inheritdoc/>
        public async Task<ModuleDefinition> GetModule(string name)
        {
            var json = await base.GetAsync<string>(Url + $"api/{ApiVersion}", "ModuleDefinition", "Get", new Dictionary<string, string> { { "name", name } });
            return JsonConvert.DeserializeObject<ModuleDefinition>(json);

        }

        /// <inheritdoc/>
        public async Task<ServiceDefinition> GetService(string name)
        {
            var json = await base.GetAsync<string>(Url + $"api/{ApiVersion}", "ServiceDefinition", "Get", new Dictionary<string, string> { { "name", name } });
            return JsonConvert.DeserializeObject<ServiceDefinition>(json);
        }

        /// <inheritdoc/>
        public async Task SaveService(ServiceDefinition service)
        {
            await base.PostAsync<Task, ServiceDefinition>(Url + $"api/{ApiVersion}", "ServiceDefinition", "Save", service);
        }

        /// <inheritdoc/>
        public async Task DeleteService(ServiceDefinition service)
        {
            await base.PostAsync<Task, ServiceDefinition>(Url + $"api/{ApiVersion}", "ServiceDefinition", "Delete", service);
        }
    }
}
