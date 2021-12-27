namespace Simplic.ServicePlatform
{
    public class ModuleBootstrapAttribute
    {
        public ModuleBootstrapAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
