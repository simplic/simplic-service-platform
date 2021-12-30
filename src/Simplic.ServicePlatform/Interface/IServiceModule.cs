using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace Simplic.ServicePlatform
{
    public interface IServiceModule
    {
        void Configure(IUnityContainer container, IServiceSession serviceSession, ServiceModuleInstance instance);
        Task Start(CancellationToken cancellationToken);
        Task Stop();
    }
}
