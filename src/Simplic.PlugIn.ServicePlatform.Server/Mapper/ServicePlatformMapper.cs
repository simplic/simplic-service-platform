using Simplic.ServicePlatform;
using Simplic.ServicePlatform.Shared;

namespace Simplic.PlugIn.ServicePlatform.Server.Mapper
{
    internal class ServicePlatformMapper : AutoMapper.Profile
    {
        public ServicePlatformMapper()
        {
            CreateMap<ModuleConfigurationDefinitionModel, ModuleConfigurationDefinition>();
            CreateMap<ModuleDefinitionModel, ModuleDefinition>();
            CreateMap<ServiceDefinitionModel, ServiceDefinition>();
            CreateMap<ServiceModuleConfigurationModel, ServiceModuleConfiguration>();
            CreateMap<ServiceModuleModel, ServiceModule>();

            CreateMap<ModuleConfigurationDefinition, ModuleConfigurationDefinitionModel>();
            CreateMap<ModuleDefinition, ModuleDefinitionModel>();
            CreateMap<ServiceDefinition, ServiceDefinitionModel>();
            CreateMap<ServiceModuleConfiguration, ServiceModuleConfigurationModel>();
            CreateMap<ServiceModule, ServiceModuleModel>();
        }
    }
}
