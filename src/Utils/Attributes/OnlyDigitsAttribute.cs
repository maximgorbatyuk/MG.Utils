using System.ComponentModel.DataAnnotations;

namespace Utils.Attributes
{
    public class OnlyDigitsAttribute : RegularExpressionAttribute
    {
        public OnlyDigitsAttribute(int length)
            : base($@"^([0-9]{{{length}}})$")
        {
        }
    }
}