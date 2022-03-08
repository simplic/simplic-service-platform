using Simplic.Framework.UI;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using Simplic.Studio.UI.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;
using System.Windows.Data;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
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
            Configure();
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

        private void Configure()
        {
            AllowPaging = false;
        }

        public override void OnSave(WindowSaveEventArg e)
        {
            if (DataContext is ServiceViewModel viewModel)
            {
                viewModel.SaveCommand.Execute(this);
            }
        }
    }
}
