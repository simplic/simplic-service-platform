﻿using System;
using System.Linq;
using System.Windows.Controls;
using Simplic.Framework.UI;
using System.Windows.Controls.Primitives;
using Simplic.Studio.UI.Navigation;
using Telerik.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Simplic.ServicePlatform.UI.Models;
using System.Windows;

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

        private void RefreshConsole()
        {
            if (DataContext is not ServiceViewModel viewModel || viewModel.SelectedServiceLog == null || !viewModel.SelectedServiceLog.Any()) return;
            var caretAtEnd = LogBox.Document.CaretPosition.IsPositionAtDocumentEnd;

            var oldServiceLog = viewModel.SelectedServiceLog.ToList();
            viewModel.RefreshServiceLogCommand.Execute(this);
            var newServiceLog = viewModel.SelectedServiceLog.ToList();

            if (oldServiceLog.Count >= newServiceLog.Count) return;

            for (var i = oldServiceLog.Count; i < newServiceLog.Count; i++)
            {
                LogBox.Document.Sections.Last.Blocks.Add(LogHelper.ToParagraph(newServiceLog[i]));
            }

            if (caretAtEnd)
                LogBox.Document.CaretPosition.MoveToDocumentEnd();
        }

        #region Event Handlers
        private void CommandBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            if (DataContext is ServiceViewModel viewModel)
            {
                viewModel.ExecuteCommand.Execute(this);
            }
        }

        private void LogBox_OnDocumentChanged(object sender, EventArgs e)
        {
            LogBox.Document.CaretPosition.MoveToDocumentEnd();
        }

        private void ServicesCardView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is not ServiceViewModel viewModel) return;

            if (viewModel.SelectedServiceLogDocument != null)
                LogBox.Document = viewModel.SelectedServiceLogDocument;
        }

        private void Filter_OnChange(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ServiceViewModel viewModel) return;

            viewModel.FilterServiceLogCommand.Execute(this);

            if (viewModel.SelectedServiceLogDocument != null)
                LogBox.Document = viewModel.SelectedServiceLogDocument;
        }
        #endregion

        private void CheckScrollbarIsDown()
        {
            var verticalOffset = LogBox.VerticalOffset;
            var viewportHeight = LogBox.ViewportHeight;
            var actualHeight = LogBox.ActualHeight;
            if (LogBox.VerticalOffset != 0)
            {
                MessageBox.Show(LogBox.VerticalOffset + LogBox.ViewportHeight == LogBox.ActualHeight
                    ? "It is  at the bottom now!"
                    : "It is not at the bottom now");
            }
        }

        /// <summary>
        /// Method that is called when the RibbonButton for saving is pressed.
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            if (DataContext is not ServiceViewModel viewModel) return;

            viewModel.SaveCommand.Execute(this);
        }

    }
}
