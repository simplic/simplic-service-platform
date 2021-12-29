using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace Simplic.ServicePlatform
{
    public interface IServiceModule
    {
        void Configure(IUnityContainer container, ServiceDefinition definition, ServiceModuleInstance instance);
        Task Start(CancellationToken cancellationToken);
        Task Stop();
    }
}
