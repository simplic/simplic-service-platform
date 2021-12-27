using System.Threading.Tasks;

namespace Simplic.ServicePlatform.Worker
{
    public interface IWorkerBase
    {
        Task Start(ServiceModuleInstance module);
        
        Task Shutdown();
    }
}
