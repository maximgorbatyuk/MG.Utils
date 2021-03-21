using System.ComponentModel.DataAnnotations;

namespace Utils.Attributes
{
    public class StandardStringLengthAttribute : StringLengthAttribute
    {
        public StandardStringLengthAttribute(int length = 255)
            : base(length)
        {
        }
    }
}