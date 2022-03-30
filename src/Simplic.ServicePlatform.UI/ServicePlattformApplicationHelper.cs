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
        /// This method gets called in simplic studio, it opens the service application configuration ui.
        /// </summary>
        /// <param name="_"></param>
        public static void ShowEditor(Guid _)
        {
            IServiceClient serviceClient = CommonServiceLocator.ServiceLocator.Current.GetInstance<IServiceClient>();
            Application.Current.Dispatcher.Invoke(async () =>
            {
                var service = await serviceClient.GetAllServices();
                var view = new ServiceView(serviceClient);
                view.Show();
            });
        }
    }
}
