using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform
{
    public class ServiceDefinitionService : IServiceDefinitionService
    {
        private readonly IServiceDefinitionRepository repository;

        public ServiceDefinitionService(IServiceDefinitionRepository repository)
        {
            this.repository = repository;
        }

        public Task Delete(string name) => repository.Delete(name);

        public Task<IList<ServiceDefinition>> GetAll() => repository.GetAll();

        public Task Save(ServiceDefinition service) => repository.Save(service);
    }
}
