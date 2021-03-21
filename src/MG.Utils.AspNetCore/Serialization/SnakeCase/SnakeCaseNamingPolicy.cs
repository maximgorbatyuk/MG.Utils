using System.Text.Json;

namespace MG.Utils.AspNetCore.Serialization.SnakeCase
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToSnakeCase();
    }
}