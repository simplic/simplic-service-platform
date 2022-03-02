using Simplic.Framework.UI;
using System.Collections.Generic;
using System.Windows;
using Simplic.Studio.UI.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServiceView : DefaultRibbonWindow 
    {
        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IServiceClient serviceClient)
        {
            InitializeComponent();
            DataContext = new ServiceViewModel(serviceClient);
            var addCardButton = new RibbonButton()
            {
                Size = Telerik.Windows.Controls.RibbonView.ButtonSize.Large,
                LargeIconName = "preProductList_add_32x",
                TextLocalizationKey = "shipment_split_window_splitbutton"
            };
        }
    }
}
