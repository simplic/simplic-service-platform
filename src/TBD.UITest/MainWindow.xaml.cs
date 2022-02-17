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

        public void OpenServiceView(object sender, EventArgs e)
        {
            var sd = new ServiceDefinition
            {
                Id = new Guid("12345678-abcd-abab-cdcd-abababababab"),
                ServiceName = "Example Service",
                Modules = new List<ServiceModule> {
                        new ServiceModule {
                            Name = "SomeServiceModule",
                            Configuration = new List<ServiceModuleConfiguration>
                            {
                                new ServiceModuleConfiguration{ Name = "Some Configuration", Value = "Some Value" },
                                new ServiceModuleConfiguration{ Name = "Some Other Configuration", Value = "Some Other Value" },
                                new ServiceModuleConfiguration{ Name = "Some Other Other Configuration", Value = "Some Other Other Value" }
                            }
                        }
                    }
            };
            var view = new ServiceView(sd);
                
            view.Show();
        }

        public void OpenModuleView(object sender, EventArgs e)
        {
            new ModuleView(
                new ModuleDefinition
                {
                    Name = "simplic.does.not.exist",
                    Description = "This plugin doesn't exist",
                    Assembly = "simplic.plugin.some.assembly",
                    EnableAutoStart = true,
                    Requires = new List<string> { "something", "something else", "someotherthing" },
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition>()
                    {
                        new ModuleConfigurationDefinition { Default = "default1", Name = "someName" },
                        new ModuleConfigurationDefinition { Default = "default2", Name = "someOtherName" }
                    }

                }).Show();
        }
    }

}
