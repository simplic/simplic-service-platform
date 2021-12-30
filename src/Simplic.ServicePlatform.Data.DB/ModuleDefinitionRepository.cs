using Newtonsoft.Json;
using Simplic.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.Data.DB
{
    /// <inheritdoc/>
    public class ModuleDefinitionRepository : IModuleDefinitionRepository
    {
        private readonly IFileService fileService;

        /// <summary>
        /// Initialize module definition repository.
        /// </summary>
        /// <param name="fileService">Repository file-service instance</param>
        public ModuleDefinitionRepository(IFileService fileService)
        {
            this.fileService = fileService;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task<ModuleDefinition> GetByName(string name)
        {
            var json = fileService.ReadAllText($"/services/definition/{name}.json");
            return Task.FromResult(JsonConvert.DeserializeObject<ModuleDefinition>(json));
        }

        /// <inheritdoc/>
        public Task Delete(string name)
        {
            fileService.Delete($"/services/definition/{name}.json");

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task Save(ModuleDefinition module)
        {
            var json = JsonConvert.SerializeObject(module);

            fileService.WriteAllText($"/services/definition/{module.Name}.json", json);

            return Task.CompletedTask;
        }
    }
}
