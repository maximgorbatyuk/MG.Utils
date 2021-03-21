namespace Utils.Random
{
    public record RandomString : RandomStringBase
    {
        public RandomString(string chars, int length)
            : base(length)
        {
            Chars = chars;
        }

        protected override string Chars { get; }
    }
}