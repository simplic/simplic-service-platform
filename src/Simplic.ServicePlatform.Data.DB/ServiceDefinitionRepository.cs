using Newtonsoft.Json;
using Simplic.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.Data.DB
{
    /// <inheritdoc/>
    public class ServiceDefinitionRepository : IServiceDefinitionRepository
    {
        private readonly IFileService fileService;

        /// <summary>
        /// Initialize definition repository
        /// </summary>
        /// <param name="fileService">Repository file-service</param>
        public ServiceDefinitionRepository(IFileService fileService)
        {
            this.fileService = fileService;
        }

        /// <inheritdoc/> 
        public Task<IList<ServiceDefinition>> GetAll()
        {
            IList<ServiceDefinition> result = new List<ServiceDefinition>();

            foreach (var file in fileService.GetFilesByPath("/services/"))
            {
                var json = fileService.ReadAllText(file.Guid);
                result.Add(JsonConvert.DeserializeObject<ServiceDefinition>(json));
            }

            return Task.FromResult(result);
        }

        /// <inheritdoc/>
        public Task<ServiceDefinition> Get(string name)
        {
            var json = fileService.ReadAllText($"/services/{name}.json");

            if (string.IsNullOrWhiteSpace(json))
                return Task.FromResult<ServiceDefinition>(null);

            return Task.FromResult(JsonConvert.DeserializeObject<ServiceDefinition>(json));
        }

        /// <inheritdoc/>
        public Task Delete(string name)
        {
            fileService.Delete($"/services/{name}.tbd.json");

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task Save(ServiceDefinition service)
        {
            var json = JsonConvert.SerializeObject(service);

            fileService.WriteAllText($"/services/{service.ServiceName}.tbd.json", json);
            //fileService.WriteAllText($@"C:\Users\schapera\Documents\Services\{service.ServiceName}.json", json);

            return Task.CompletedTask;
        }
    }
}
