using System;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for ModuleView.xaml
    /// </summary>
    public partial class ModuleView : Window
    {
        public ModuleView(ModuleDefinition moduleDefinition)
        {
            InitializeComponent();
            DataContext = new ModuleViewModel(moduleDefinition);
        }

        private void EnableAutoStart_Changed(object sender, EventArgs e)
        {
            ModuleViewModel viewModel = (ModuleViewModel)DataContext;
            if (viewModel == null)
                return;
            EnableAutoStartText.Content = viewModel.EnableAutoStart ? "ON" : "OFF";
        }
    }
}
