using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Represents a service module. <see cref="ServiceModuleAttribute"/> must be used for making the service module visible to the application server.
    /// </summary>
    public interface IServiceModule
    {
        /// <summary>
        /// Method to configure the actual service module
        /// </summary>
        /// <param name="container">Unity container instance</param>
        /// <param name="serviceSession">Actual service session</param>
        /// <param name="instance">Actual service-module with all configuration values</param>
        void Configure(IUnityContainer container, IServiceSession serviceSession, ServiceModuleInstance instance);

        /// <summary>
        /// Will be called to start the actual service module. A main loop can be started in a separat thread or task in here.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Start(CancellationToken cancellationToken);

        /// <summary>
        /// Will be called before the application server stops.
        /// </summary>
        Task Stop();
    }
}
