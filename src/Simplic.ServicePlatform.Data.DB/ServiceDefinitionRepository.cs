using Newtonsoft.Json;
using Simplic.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.Data.DB
{
    public class ServiceDefinitionRepository : IServiceDefinitionRepository
    {
        private readonly IFileService fileService;

        public ServiceDefinitionRepository(IFileService fileService)
        {
            this.fileService = fileService;
        }

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

        public Task<ServiceDefinition> Get(string name)
        {
            var json = fileService.ReadAllText($"/services/{name}.json");
            return Task.FromResult(JsonConvert.DeserializeObject<ServiceDefinition>(json));
        }

        public Task Delete(string name)
        {
            fileService.Delete($"/services/{name}.json");

            return Task.CompletedTask;
        }

        public Task Save(ServiceDefinition service)
        {
            var json = JsonConvert.SerializeObject(service);

            fileService.WriteAllText($"/services/{service.ServiceName}.json", json);

            return Task.CompletedTask;
        }
    }
}
