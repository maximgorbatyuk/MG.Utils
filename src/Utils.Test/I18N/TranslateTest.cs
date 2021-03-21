using System;
using System.Globalization;
using Utils.I18N;
using Xunit;

namespace Utils.Test.I18N
{
    public class TranslateTest
    {
        [Theory]
        [InlineData("HelloKey", "Hello", "Привет", "en", "Hello")]
        [InlineData("HelloKey", "Hello", "Привет", "ru", "Привет")]
        [InlineData("HelloKey", "Hello", "", "ru", "Hello")]
        [InlineData("HelloKey", "Hello", null, "ru", "Hello")]
        [InlineData("HelloKey", "", "Привет", "en", "HelloKey")]
        [InlineData("HelloKey", null, "Привет", "ru", "Привет")]
        public void TranslationByName_HasValue_Ok(string key, string en, string ru, string culture, string expectedValue)
        {
            var target = new Translate
            {
                Key = key,
                en = en,
                ru = ru
            };

            Assert.Equal(expectedValue, target.TranslationByName(new CultureInfo(culture)));
        }

        [Theory]
        [InlineData("HelloKey", "Hello", "Привет", true)]
        [InlineData("HelloKey", "", "Привет", false)]
        [InlineData("HelloKey", "Hello", "", false)]
        [InlineData("HelloKey", "", "", false)]
        [InlineData("HelloKey", null, "Привет", false)]
        [InlineData("HelloKey", "Hello", null, false)]
        [InlineData("HelloKey", null, null, false)]
        public void IsValid_Cases(
            string key, string en, string ru, bool expectedValue)
        {
            var target = new Translate
            {
                Key = key,
                en = en,
                ru = ru
            };

            Assert.Equal(expectedValue, target.IsValid());
        }

        [Theory]
        [InlineData("HelloKey", "Hello", "Привет", true)]
        [InlineData("HelloKey", "", "Привет", true)]
        [InlineData("HelloKey", "Hello", "", true)]
        [InlineData("HelloKey", "", "", false)]
        [InlineData("HelloKey", null, "Привет", true)]
        [InlineData("HelloKey", "Hello", null, true)]
        [InlineData("HelloKey", null, null, false)]
        public void HasAnyValue_Cases(
            string key, string en, string ru, bool expectedValue)
        {
            var target = new Translate
            {
                Key = key,
                en = en,
                ru = ru
            };

            Assert.Equal(expectedValue, target.HasAnyValue);
        }

        [Theory]
        [InlineData("HelloKey", "Hello", "Привет", "fr")]
        [InlineData("HelloKey", "Hello", "Привет", "jp")]
        public void TranslationByName_Cases(
            string key, string en, string ru, string culture)
        {
            var target = new Translate
            {
                Key = key,
                en = en,
                ru = ru
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => target.TranslationByName(new CultureInfo(culture)));
        }
    }
}