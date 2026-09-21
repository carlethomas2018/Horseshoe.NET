using System;
using System.Collections.Generic;
using System.Linq;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Collections
{
    public static class ListUtil
    {
        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to a <see cref="List{T}"/>. If the collection is already a list, it is returned as-is (optimization).
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">The collection to convert.</param>
        /// <returns>The converted or original list.</returns>
        public static List<T> AsList<T>(IEnumerable<T> collection)
        {
            if (collection is List<T> list)
                return list;

            return collection == null
                ? new List<T>()
                : new List<T>(collection);
        }

        /// <summary>
        /// Fits a list to a specified length by adding or removing elements as necessary. 
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">The collection to fit.</param>
        /// <param name="length">The desired length of the list.</param>
        /// <param name="value">The value to use when filling the list.</param>
        /// <param name="locale">The locale for localization.</param>
        /// <returns>The fitted list.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static List<T> Fit<T>(IEnumerable<T> collection, int length, T value = default, string locale = null)
        {
            if (collection == null)
                collection = Array.Empty<T>();

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), Lang.Get("List.Length.Negative", locale: locale));

            if (length == collection.Count())
                return AsList(collection);

            var list = new List<T>(collection);
            while (list.Count != length)
            {
                if (list.Count < length)
                {
                    list.Add(value);
                }
                else
                {
                    list.RemoveAt(list.Count - 1);
                }
            }

            return list;
        }

        private static Languages Lang { get; } = new Languages
        {
            { "List.Length.Negative", "Length cannot be negative." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "List.Length.Negative", "La longitud no puede ser negativa." },
            }
        );
    }
}
