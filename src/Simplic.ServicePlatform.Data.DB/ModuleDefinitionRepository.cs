using Newtonsoft.Json;
using Simplic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.Data.DB
{
    public class ModuleDefinitionRepository : IModuleDefinitionRepository
    {
        private readonly IFileService fileService;

        public ModuleDefinitionRepository(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public Task<IList<ModuleDefinition>> GetAll()
        {
            IList<ModuleDefinition> result = new List<ModuleDefinition>();

            foreach (var file in fileService.GetFilesByPath("/services/definition/"))
            {
                var json = fileService.ReadAllText(file.Guid);
                result.Add(JsonConvert.DeserializeObject<ModuleDefinition>(json));
            }

            return Task.FromResult(result);
        }

        public Task<ModuleDefinition> GetByName(string name)
        {
            var json = fileService.ReadAllText($"/services/definition/{name}.json");
            return Task.FromResult(JsonConvert.DeserializeObject<ModuleDefinition>(json));
        }

        public Task Delete(string name)
        {
            fileService.Delete($"/services/definition/{name}.json");

            return Task.CompletedTask;
        }

        public Task Save(ModuleDefinition module)
        {
            var json = JsonConvert.SerializeObject(module);

            fileService.WriteAllText($"/services/definition/{module.Name}.json", json);

            return Task.CompletedTask;
        }
    }
}
