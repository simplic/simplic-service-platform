using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
