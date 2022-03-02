using Simplic.Framework.UI;
using System.Collections.Generic;
using System.Windows;
using Simplic.Studio.UI.Navigation;
using Telerik.Windows.Controls;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServiceView : DefaultRibbonWindow 
    {
        private readonly IServiceClient serviceClient;
        private ServiceViewModel viewModel;
        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IServiceClient serviceClient)
        {
            InitializeComponent();
            this.serviceClient = serviceClient;
            this.viewModel = new ServiceViewModel(serviceClient);
            DataContext = this.viewModel;
            var addCardButton = new RibbonButton()
            {
                Size = Telerik.Windows.Controls.RibbonView.ButtonSize.Large,
                LargeIconName = "preProductList_add_32x",
                TextLocalizationKey = "shipment_split_window_splitbutton"
            };
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
