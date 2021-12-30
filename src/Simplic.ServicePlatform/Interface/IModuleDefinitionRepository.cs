using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Repository for reading and writing module definitions.
    /// </summary>
    public interface IModuleDefinitionRepository
    {
        /// <summary>
        /// Gets all available module definitions
        /// </summary>
        /// <returns>List of module definitions</returns>
        Task<IList<ModuleDefinition>> GetAll();

        /// <summary>
        /// Gets a specific module definition by its name.
        /// </summary>
        /// <param name="name">Name of the module definition</param>
        /// <returns>Module definition if exists, else null</returns>
        Task<ModuleDefinition> GetByName(string name);

        /// <summary>
        /// Remove a module definition by its name.
        /// </summary>
        /// <param name="name"></param>
        Task Delete(string name);

        /// <summary>
        /// Create or update module definition
        /// </summary>
        /// <param name="module">Module definition instance</param>
        Task Save(ModuleDefinition module);
    }
}
