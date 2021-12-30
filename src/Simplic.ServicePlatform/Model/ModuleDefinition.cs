using System;
using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ModuleDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Assembly { get; set; }
        public bool EnableAutoStart { get; set; }
        public IList<string> Requires { get; set; } = new List<string>();
        public IList<ModuleConfigurationDefinition> ConfigurationDefinition { get; set; } = new List<ModuleConfigurationDefinition>();
    }
}
