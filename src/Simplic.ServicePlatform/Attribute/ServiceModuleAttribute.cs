using System;

namespace Simplic.ServicePlatform
{
    /// <summary>
    /// Must be placed for the registration of a <see cref="IServiceModule"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceModuleAttribute : Attribute
    {
        /// <summary>
        /// Initialize service module attribute
        /// </summary>
        /// <param name="name">Module name</param>
        public ServiceModuleAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the module name.
        /// <para>Must be the same name as in the parameter module definition/json file.</para>
        /// </summary>
        public string Name { get; }
    }
}
