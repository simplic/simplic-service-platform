using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    public static class ServicePlattformApplicationHelper
    {

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
