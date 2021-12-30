using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.Service
{
    /// <inheritdoc/>
    public class ModuleDefinitionService : IModuleDefinitionService
    {
        private readonly IModuleDefinitionRepository moduleDefinitionRepository;

        /// <summary>
        /// Initialize module definition service
        /// </summary>
        /// <param name="moduleDefinitionRepository">Repository instance</param>
        public ModuleDefinitionService(IModuleDefinitionRepository moduleDefinitionRepository)
        {
            this.moduleDefinitionRepository = moduleDefinitionRepository;
        }

        /// <inheritdoc/>
        public Task Delete(string name) => moduleDefinitionRepository.Delete(name);

        /// <inheritdoc/>
        public Task<IList<ModuleDefinition>> GetAll() => moduleDefinitionRepository.GetAll();

        /// <inheritdoc/>
        public Task<ModuleDefinition> GetByName(string name) => moduleDefinitionRepository.GetByName(name);

        /// <inheritdoc/>
        public Task Save(ModuleDefinition module) => moduleDefinitionRepository.Save(module);
    }
}
