using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PSMDesktopUI
{
    public static class EnumHelper
    {
        public static string Description(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            
            if (attributes.Any())
            {
                return (attributes.First() as DescriptionAttribute).Description;
            }

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string str = Regex.Replace(value.ToString(), "[A-Z]", " $0");

            return textInfo.ToTitleCase(str);
        }

        public static IEnumerable<KeyValuePair<Enum, string>> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
            {
                throw new ArgumentException($"{nameof(t)} must be an enum type");
            }

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new KeyValuePair<Enum, string>(e, e.Description())).ToList();
        }
    }
}
