using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Simplic.ServicePlatform.UI
{
    public class SSPViewModelBase : INotifyPropertyChanged
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

        /// <summary>
        /// Parse module name to a more presentable format.
        /// </summary>
        /// <param name="name">Module name</param>
        /// <returns>Parsed name</returns>
        protected string ParseModuleName(string name)
        {
            var nameBuilder = new StringBuilder(name);
            nameBuilder[0] -= (char)32;
            for (int i = 0; i < name.Length; i++)
                if (name[i] == '.')
                {
                    nameBuilder[i] = ' ';
                    if (i + 1 < name.Length)
                        nameBuilder[i + 1] = (char)(nameBuilder[i + 1] - 32);
                }

            return nameBuilder.ToString();
        }
    }
}
