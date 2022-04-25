using System;
using System.IO;
using Simplic.Framework.UI;
using System.Windows.Controls.Primitives;
using Simplic.Studio.UI.Navigation;
using Telerik.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using Telerik.Windows.Documents.Model;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for ServiceView.xaml
    /// </summary>
    public partial class ServiceView : DefaultRibbonWindow
    {
        private RibbonButton addCardButton;
        private RibbonButton removeCardButton;
        private readonly DispatcherTimer timer;

        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IServiceClient serviceClient)
        {
            InitializeComponent();
            DataContext = new ServiceViewModel(serviceClient);
            CreateButtons();
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50),
                IsEnabled = true
            };
            timer.Tick += (sender, args) => RefreshConsole();
            timer.Start();
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


        private void CommandBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            if (DataContext is ServiceViewModel viewModel)
            {
                viewModel.ExecuteCommand.Execute(this);
            }
        }

        private RadDocument ImportRadDocumentXaml(string xaml)
        {
            var stringReader = new StringReader(xaml);
            var xmlReader = XmlReader.Create(stringReader);
            return (RadDocument)XamlReader.Load(xmlReader);
        }

        private void RefreshConsole()
        {
            if (DataContext is ServiceViewModel viewModel && !string.IsNullOrWhiteSpace(viewModel.SelectedServiceLogXaml))
            {
                LogBox.Document = ImportRadDocumentXaml(viewModel.SelectedServiceLogXaml);
            }
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
