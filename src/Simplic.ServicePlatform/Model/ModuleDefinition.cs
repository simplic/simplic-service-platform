using System;
using System.Collections.Generic;

namespace Simplic.ServicePlatform
{
    public class ModuleDefinition
    {
        [Obsolete("Legacy application server module")]
        public const string LegacyModuleType = "legacy_module";

        [Obsolete("Legacy application server python module")]
        public const string LegacyModulePythonType = "legacy_python";
        public const string StatelessType = "stateless";
        public const string WorkerType = "worker";

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Startup { get; set; }
        public string Worker { get; set; }
        public IList<ModuleConfigurationDefinition> ConfigurationDefinition { get; set; } = new List<ModuleConfigurationDefinition>();
    }
}
