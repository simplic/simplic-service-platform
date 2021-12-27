using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public interface IModuleDefinitionRepository
    {
        Task<IList<ModuleDefinition>> GetAll();
        Task<ModuleDefinition> GetByName(string name);
        Task Delete(string name);
        Task Save(ModuleDefinition module);
    }
}
