using System;
using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ModuleDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Assembly { get; set; }
        public IList<string> Required { get; set; } = new List<string>();
        public IList<ModuleConfigurationDefinition> ConfigurationDefinition { get; set; } = new List<ModuleConfigurationDefinition>();
    }
}
