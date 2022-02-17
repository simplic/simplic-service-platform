using Simplic.ServicePlatform.Data.DB;
using Simplic.ServicePlatform.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServiceView : Window
    {
        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IModuleDefinitionService moduleDefinitionService, ServiceDefinition serviceDefinition)
        {
            InitializeComponent();
            DataContext = new ServiceViewModel(moduleDefinitionService, serviceDefinition);
        }
    }
}
