using System;
using System.Linq;
using Unity;
using Unity.Lifetime;

namespace Simplic.ServicePlatform.Service
{
    /// <inheritdoc/>
    public class ServiceModuleLookupService : IServiceModuleLookupService
    {
        private readonly IServiceSession serviceSession;
        private readonly IUnityContainer unityContainer;

        /// <summary>
        /// Initialize lookup service
        /// </summary>
        /// <param name="unityContainer">Unity container</param>
        /// <param name="serviceSession">Actual service session instance</param>
        public ServiceModuleLookupService(IUnityContainer unityContainer, IServiceSession serviceSession)
        {
            this.serviceSession = serviceSession;
            this.unityContainer = unityContainer;
        }
        
        /// <inheritdoc/>
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