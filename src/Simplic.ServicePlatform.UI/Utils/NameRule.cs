using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Simplic.ServicePlatform.UI.Utils
{
    public class NameRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string))
                return new ValidationResult(false, "oaksd");

            var name = value as string;
            if (!Regex.IsMatch(name, $@"[A-Za-z]{{{MinimumCharacters},{MaximumCharacters}}}"))
                return new ValidationResult(false, $"Name has to be between {MinimumCharacters} and {MaximumCharacters} characters long.");

            return new ValidationResult(true, null);
        }

        public string MinimumCharacters { get; set; }
        public string MaximumCharacters { get; set; }
        public bool AllowNumbers { get; set; }
    }
}
