using System;
using MG.Utils.Helpers;

namespace MG.Utils.ValueObjects
{
    public record NonNullableInt
    {
        private readonly string _source;
        private int? _value;

        /// <summary>
        /// Null is not allowed.
        /// </summary>
        /// <param name="source">Source.</param>
        public NonNullableInt(string source)
        {
            _source = source;
        }

        public int ToInt()
        {
            if (_value == null)
            {
                _source.ThrowIfNull(nameof(_source));

                if (int.TryParse(_source, out var result))
                {
                    _value = result;
                }
                else
                {
                    throw new InvalidCastException($"Could not cast '{_source}' as integer");
                }
            }

            return _value.Value;
        }

        public bool Equals(long second) => second == ToInt();

        public static implicit operator int(NonNullableInt nnInt) => nnInt.ToInt();
    }
}