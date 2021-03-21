﻿namespace MG.Utils.Random
{
    public record RandomDigital : RandomStringBase
    {
        public RandomDigital(int length)
            : base(length)
        {
        }

        protected override string Chars => "0123456789";
    }
}