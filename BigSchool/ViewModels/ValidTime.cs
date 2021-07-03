using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BigSchool.ViewModel
{
    public class ValidTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime;
            var isValid = DateTime.TryParseExact(
                Convert.ToString(value),
                "H:mm",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dateTime);
            return isValid;
        }
    }
    
}