namespace Utils.Attributes
{
    public class PhoneNumberAttribute : OnlyDigitsAttribute
    {
        public const int Length = 11;

        public PhoneNumberAttribute()
            : base(Length)
        {
        }
    }
}