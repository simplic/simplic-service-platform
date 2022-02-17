using Simplic.ServicePlatform;
using Simplic.ServicePlatform.UI;
using System;
using System.Collections.Generic;
using System.Windows;

namespace TBD.UITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationHelper helper;
        public MainWindow()
        {
            InitializeComponent();
            helper = new ApplicationHelper();
        }

        public void OpenServiceView(object sender, EventArgs e)
        {
            helper.ShowWindow(0);
        }

        public void OpenModuleView(object sender, EventArgs e)
        {
            helper.ShowWindow(1);
        }
    }

}
