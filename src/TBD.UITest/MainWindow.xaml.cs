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
        public MainWindow()
        {
            InitializeComponent();
        }
        public void testView(object sender, EventArgs e)
        {
            new ServiceManagerView(
                new ModuleDefinition()
                {
                    Name = "simplic.does.not.exist",
                    Description = "This plugin doesn't exist",
                    Assembly = "simplic.plugin.doesntexist",
                    EnableAutoStart = true,
                    Requires = new List<string>() { "a", "s", "d"},
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition>()
                    {
                        new ModuleConfigurationDefinition() { Default = "default1", Name = "someName" },
                        new ModuleConfigurationDefinition() { Default = "default2", Name = "someOtherName" }
                    }

                }
                ).Show();
        }
    }

}
