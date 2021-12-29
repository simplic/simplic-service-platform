using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Lifetime;

namespace Simplic.ServicePlatform.Service
{
    public class ServiceModuleLookupService : IServiceModuleLookupService
    {
        private readonly IServiceSession serviceSession;
        private readonly IUnityContainer unityContainer;

        public ServiceModuleLookupService(IServiceSession serviceSession, IUnityContainer unityContainer)
        {
            this.serviceSession = serviceSession;
            this.unityContainer = unityContainer;
        }

        public void RegisterServices()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                                                            .Where(x => serviceSession.Modules.Any(y => y.Assembly == x.GetName().Name)))
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attriutes = Attribute.GetCustomAttributes(type);

                    foreach (var attribute in attriutes?.OfType<ServiceModuleAttribute>())
                    {
                        // Registr container
                        unityContainer.RegisterType(typeof(IServiceModule), type, attribute.Name, lifetimeManager: new ContainerControlledLifetimeManager());
                    }
                }
            }
        }
    }
}