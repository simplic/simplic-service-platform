using System;
using System.Collections.Generic;
using Simplic.ServicePlatform;

namespace Simplic.ServicePlatform.Shared
{
    public class ServiceDefinitionDataModel
    {
        public IList<ServiceDefinitionModel> Services { get; set; } = new List<ServiceDefinitionModel       >();
        public IList<ModuleDefinitionModel> Definitions { get; set; } = new List<ModuleDefinitionModel>();
        public Guid Id { get; set; }
    }
}
