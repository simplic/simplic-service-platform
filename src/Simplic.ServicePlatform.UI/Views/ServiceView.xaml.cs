using Simplic.Framework.UI;
using System.Windows.Controls.Primitives;
using Simplic.Studio.UI.Navigation;
using Telerik.Windows.Controls;
using System.Windows.Data;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for ServiceView.xaml
    /// </summary>
    public partial class ServiceView : DefaultRibbonWindow
    {
        private RibbonButton addCardButton;
        private RibbonButton removeCardButton;

        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IServiceClient serviceClient)
        {
            InitializeComponent();
            DataContext = new ServiceViewModel(serviceClient);
            CreateButtons();
        }

        private void CreateButtons()
        {
            var cardButtonGroup = new RadRibbonGroup { Header = "Service" };

            addCardButton = new RibbonButton
            {
                Text = "Add",
                Size = Telerik.Windows.Controls.RibbonView.ButtonSize.Large,
                LargeIconName = "service_add_32x",
                TextLocalizationKey = "xaml_add"
            };

            cardButtonGroup.Items.Add(addCardButton);

            removeCardButton = new RibbonButton
            {
                Text = "Remove",
                Size = Telerik.Windows.Controls.RibbonView.ButtonSize.Large,
                LargeIconName = "delete_32x",
                TextLocalizationKey = "kanban_remove"
            };

            BindingOperations.SetBinding(removeCardButton, ButtonBase.CommandProperty, new Binding(nameof(ServiceViewModel.DeleteCardCommand))
            {
                Source = DataContext
            });

            cardButtonGroup.Items.Add(removeCardButton);

            if (DataContext is ServiceViewModel viewModel)
            {

                addCardButton.Command = viewModel.AddCardCommand;
            }

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

    }
}
