using Simplic.Framework.UI;
using Simplic.Studio.UI.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RibbonView;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for ServiceView.xaml
    /// </summary>
    public partial class ServiceView : DefaultRibbonWindow
    {
        private readonly ServiceViewModel viewModel;
        
        private RibbonButton addCardButton;
        private RibbonButton removeCardButton;
        private RibbonButton collapseAllButton;
        private RibbonButton expandAllButton;

        private RadCardView services;

        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IServiceClient serviceClient)
        {
            InitializeComponent();
            viewModel = new ServiceViewModel(serviceClient);
            viewModel.CollapseEvent += OnCollapse;
            viewModel.ExpandEvent += OnExpand;
            services = (RadCardView)FindName("ServicesCardView");
            DataContext = viewModel;
            CreateButtons();
        }

        private void CreateButtons()
        {
            var cardButtonGroup = new RadRibbonGroup { Header = "Service" };

            addCardButton = new RibbonButton
            {
                Size = ButtonSize.Large,
                LargeIconName = "service_add_32x",
                TextLocalizationKey = "xaml_add"
            };

            cardButtonGroup.Items.Add(addCardButton);

            removeCardButton = new RibbonButton
            {
                Size = ButtonSize.Large,
                LargeIconName = "delete_32x",
                TextLocalizationKey = "kanban_remove"
            };

            cardButtonGroup.Items.Add(removeCardButton);

            collapseAllButton = new RibbonButton
            {
                Size = ButtonSize.Large,
                LargeIconName = "usermanagement_collapse_all_32x",
                TextLocalizationKey = "ssp_collapse_all"
            };

            cardButtonGroup.Items.Add(collapseAllButton);

            expandAllButton = new RibbonButton
            {
                Size = ButtonSize.Large,
                LargeIconName = "usermanagement_expand_all_32x",
                TextLocalizationKey = "ssp_expand_all"
            };

            cardButtonGroup.Items.Add(expandAllButton);

            addCardButton.Command = viewModel.AddCardCommand;
            removeCardButton.Command = viewModel.DeleteCardCommand;
            collapseAllButton.Command = viewModel.CollapseAllCommand;
            expandAllButton.Command = viewModel.ExpandAllCommand;

            RadRibbonHomeTab.Items.Add(cardButtonGroup);
        }

        /// <summary>
        /// Method that is called when the RibbonButton for saving is pressed.
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            if (DataContext is ServiceViewModel viewModel)
            {
                viewModel.SaveCommand.Execute(this);
            }
        }

        private void OnCollapse(object sender, CollapseEventArgs args)
        {
            services.Collapse(args.Service);
        }
        private void OnExpand(object sender, CollapseEventArgs args)
        {
            services.Expand(args.Service);
        }

    }
}
