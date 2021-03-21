using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Utils.Helpers;

namespace Utils.ValueObjects
{
    public record Sha256String
    {
        public Sha256String(string value)
        {
            value.ThrowIfNull(nameof(value));

            using var hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(
                Encoding.UTF8.GetBytes(value));

            Value = hash
                .Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }

        public string Value { get; }

        public static explicit operator string(Sha256String sha)
        {
            return sha?.Value;
        }
    }
}