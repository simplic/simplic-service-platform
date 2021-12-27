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

        public Task Delete(string name)
        {
            fileService.Delete($"/services/{name}.json");

            return Task.CompletedTask;
        }

        public Task Save(ServiceDefinition service)
        {
            var json = JsonConvert.SerializeObject(service);

            fileService.WriteAllText($"/services/{service.MachineName}_{service.ServiceName}_{service.Section ?? "Default"}.json", json);

            return Task.CompletedTask;
        }
    }
}
