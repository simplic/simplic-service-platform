using System;
using System.Threading.Tasks;
using Simplic.ServicePlatform;
using Simplic.Collaboration;
using System.Linq;
using AutoMapper;
using Unity;
using Simplic.ServicePlatform.Shared;

namespace Simplic.PlugIn.ServicePlatform.Server
{
    public class ServicePlatformHub : Collaboration.Hub.CollaborationHub<IServicePlatformClient, ServiceDefinitionDataModel>
    {
        private readonly IServiceDefinitionService serviceDefinitionService;
        private readonly IModuleDefinitionService moduleDefinitionService;
        private readonly IMapper mapper;

        public ServicePlatformHub(IDataSessionService dataSessionService
                                , IServiceDefinitionService serviceDefinitionService
                                , IModuleDefinitionService moduleDefinitionService
                                , [Dependency("ServicePlatform")] IMapper mapper) : base(dataSessionService)
        {
            this.serviceDefinitionService = serviceDefinitionService;
            this.moduleDefinitionService = moduleDefinitionService;
            this.mapper = mapper;
        }

        protected override async Task<ServiceDefinitionDataModel> GetData(Guid id)
        {
            var data = new ServiceDefinitionDataModel
            {
                Id = id,
                Services = (await serviceDefinitionService.GetAll()).Select(x => mapper.Map<ServiceDefinitionModel>(x)).ToList(),
                Definitions = (await moduleDefinitionService.GetAll()).Select(x => mapper.Map<ModuleDefinitionModel>(x)).ToList()
            };

            return data;
        }

        protected override async Task SaveData(ServiceDefinitionDataModel data)
        {
            foreach (var service in data.Services)
                await serviceDefinitionService.Save(mapper.Map<ServiceDefinition>(service));

            ///// foreach (var oldServices in await serviceDefinitionService.GetAll())
            ///// {
            /////     if (data.Services.Any(x => x.Id == oldServices.Id))
            /////         await serviceDefinitionService.Delete(oldServices.Id);
            ///// }
        }
    }
}
