using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Simplic.ServicePlatform.UI
{
    public class RegexTextBox : RadWatermarkTextBox
    {
        private string text;
        public string this[string name]
        {
            get => name; set => this[name] = value;
        }
        static RegexTextBox()
        {
            
        }
        public new string Text
        {
            get => text;
            set
            {
                if (!CheckFormat(value))
                {
                    ShowErrorMsg();
                    return;
                }
                text = value;

            }
        }

        private bool CheckFormat(string str)
        {
            if (!string.IsNullOrEmpty(str))
                return true;
            return false;
        }

        private void ShowErrorMsg()
        {

        }

        //public static readonly DependencyProperty TextProperty
        //    = DependencyProperty.Register("Text", typeof(string), typeof(RegexTextBox), new FrameworkPropertyMetadata(""));

        //public static readonly DependencyProperty RegexProperty
        //    = DependencyProperty.Register("Regex", typeof(string), typeof(RegexTextBox), new )
    }
}
