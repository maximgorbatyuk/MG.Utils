﻿namespace Utils.Attributes
{
    public class BinOrIinNumber : OnlyDigitsAttribute
    {
        public BinOrIinNumber()
            : base(12)
        {
        }
    }
}