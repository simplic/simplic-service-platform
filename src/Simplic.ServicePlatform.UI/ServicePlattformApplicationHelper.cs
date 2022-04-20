using System;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Static class with method to open service view ui in simplic studio.
    /// </summary>
    public static class ServicePlattformApplicationHelper
    {
        /// <summary>
        /// Opens the service application configuration ui. <br/>
        /// Called from Simplic Studio.
        /// </summary>
        /// <param name="_"></param>
        public static void ShowEditor(Guid _)
        {
            var serviceClient = CommonServiceLocator.ServiceLocator.Current.GetInstance<IServiceClient>();
            var view = new ServiceView(serviceClient);
            view.Show();
        }
    }
}
