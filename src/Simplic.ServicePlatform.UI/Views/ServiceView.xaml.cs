using System.Collections.Generic;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServiceView : Window
    {
        private readonly IServiceClient serviceClient;

        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IServiceClient serviceClient)
        {
            InitializeComponent();
            this.serviceClient = serviceClient;
            DataContext = new ServiceViewModel(serviceClient);
        }

        private ModuleDefinition ServiceModuleToModuleDefinition(ServiceModule serviceModule)
        {
            ModuleDefinition newModuleDefinition = new ModuleDefinition();
            Application.Current.Dispatcher.Invoke(async () =>
            {
                newModuleDefinition = await serviceClient.GetModule(serviceModule.Name);
            });
            return newModuleDefinition;
        }
    }
}
