using Simplic.Configuration;
using Simplic.WebApi.Client;

namespace Simplic.ServicePlatform.Client
{
    public class ServicePlatformClient : Collaboration.Client.CollaborationClient<Shared.ServiceDefinitionDataModel>
    {
        public ServicePlatformClient(IClient client, IConnectionConfigurationService connectionConfigurationService) : base(client, connectionConfigurationService)
        {

        }

        public override string HubName => "ServicePlatform";
    }
}
