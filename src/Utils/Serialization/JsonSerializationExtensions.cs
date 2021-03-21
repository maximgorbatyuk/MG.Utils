using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Utils.Helpers;

namespace Utils.Serialization
{
    public static class JsonSerializationExtensions
    {
        private static readonly SnakeCaseNamingStrategy _snakeCaseNamingStrategy
            = new SnakeCaseNamingStrategy();

        private static readonly JsonSerializerSettings _snakeCaseSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = _snakeCaseNamingStrategy
            }
        };

        public static string ToSnakeCase<T>(this T instance)
        {
            return JsonConvert.SerializeObject(instance, _snakeCaseSettings);
        }

        // TODO Maxim: tests
        // https://stackoverflow.com/a/58575386
        public static string ToSnakeCase(this string @string)
        {
            @string.ThrowIfNull(nameof(@string));
            return _snakeCaseNamingStrategy.GetPropertyName(@string, false);
        }
    }
}