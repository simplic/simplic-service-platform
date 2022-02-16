using Simplic.ServicePlatform.Data.DB;
using Simplic.ServicePlatform.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServiceManagerView : Window
    {
        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceManagerView(ModuleDefinition moduleDefinition)
        {
            //DataContext = new ServiceManagerViewModel(new ModuleDefinitionService(new ModuleDefinitionRepository(null)));
            InitializeComponent();
            titlebar.Text = ParseModuleName(moduleDefinition.Name);
        }

        /// <summary>
        /// Parse module name to a more presentable format.
        /// </summary>
        /// <param name="name">Module name</param>
        /// <returns>Parsed name</returns>
        private string ParseModuleName(string name)
        {
            var nameBuilder = new StringBuilder(name);
            nameBuilder[0] -= (char)32;
            for (int i = 0; i < name.Length; i++)
                if (name[i] == '.')
                {
                    nameBuilder[i] = ' ';
                    if (i + 1 < name.Length)
                        nameBuilder[i + 1] = (char)(nameBuilder[i + 1] - 32);
                }

            return nameBuilder.ToString();
        }
    }
}
