using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Immutable
{
    /// <summary>
    /// This is an extremley minimal interpretation of the .NET ImmutableList that does just enough to imitate it for our needs (is instantiated in the same way and can be examined in the same way - via GetEnumerator)
    /// </summary>
    public sealed class ImmutableList<T> : IEnumerable<T>
    {
        public static ImmutableList<T> Empty { get; } = new ImmutableList<T>(Array.Empty<T>());

        private readonly T[] _items;
        private ImmutableList(T[] items) => _items = items;

        public ImmutableList<T> AddRange(IEnumerable<T> items) => new ImmutableList<T>(_items.Concat(items ?? Array.Empty<T>()).ToArray());

        public IEnumerator<T> GetEnumerator() => _items.ToEnumerator<T>();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}