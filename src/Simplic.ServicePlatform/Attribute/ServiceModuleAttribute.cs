using System;

namespace Simplic.ServicePlatform
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceModuleAttribute : Attribute
    {
        public ServiceModuleAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
