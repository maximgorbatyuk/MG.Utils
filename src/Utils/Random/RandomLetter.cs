namespace MG.Utils.Random
{
    public record RandomLetter : RandomStringBase
    {
        public RandomLetter(int length)
            : base(length)
        {
        }

        protected override string Chars => "abcdefghijklmnopqrstuvwxyz";
    }
}