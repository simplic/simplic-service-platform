using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.UI.ViewModels
{
    public class AddCardViewModel
    {
            
        private string cardName;

        public AddCardViewModel()
        {
            

        }
        public string CardName { get => cardName; set => cardName = value; }
        }
}
