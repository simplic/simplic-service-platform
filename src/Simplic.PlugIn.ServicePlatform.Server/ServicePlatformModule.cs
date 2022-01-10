using Simplic.ServicePlatform;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Unity;

namespace Simplic.PlugIn.ServicePlatform.Server
{
    [ServiceModule("simplic.service_platform")]
    public class ServicePlatformModule : IServiceModule
    {
        public void Configure(Unity.IUnityContainer container, IServiceSession serviceSession, ServiceModuleInstance instance)
        {
            container.RegisterInstance("ServicePlatform", new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Mapper.ServicePlatformMapper());
            }).CreateMapper());

            container.RegisterFactory<Microsoft.AspNet.SignalR.IHubContext<IServicePlatformClient>>((_) =>
            {
                return Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServicePlatformHub, IServicePlatformClient>();
            }, new Unity.Lifetime.ContainerControlledLifetimeManager());
        }

        public async Task Start(CancellationToken cancellationToken)
        {

        }

        public async Task Stop()
        {

        }
    }
}
