using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Simplic.ServicePlatform.UI
{
    internal class ServiceManagerViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// EventHandler for the PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the PropertyChanged event for the given caller.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
