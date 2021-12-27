using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public class ModuleDefinitionService : IModuleDefinitionService
    {
        private readonly IModuleDefinitionRepository moduleDefinitionRepository;

        public ModuleDefinitionService(IModuleDefinitionRepository moduleDefinitionRepository)
        {
            this.moduleDefinitionRepository = moduleDefinitionRepository;
        }

        public Task Delete(string name) => moduleDefinitionRepository.Delete(name);

        public Task<IList<ModuleDefinition>> GetAll() => moduleDefinitionRepository.GetAll();

        public Task<ModuleDefinition> GetByName(string name) => moduleDefinitionRepository.GetByName(name);

        public Task Save(ModuleDefinition module) => moduleDefinitionRepository.Save(module);
    }
}
