using System;
using System.Collections.Generic;
using System.Linq;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Collections
{
    public static class ArrayUtil
    {
        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to a <see cref="List{T}"/>. If the collection is already a list, it is returned as-is (optimization).
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">The collection to convert.</param>
        /// <returns>The converted or original list.</returns>
        public static T[] AsArray<T>(IEnumerable<T> collection)
        {
            if (collection is T[] array)
                return array;
            return new List<T>(collection)
                .ToArray();
        }

        /// <summary>
        /// Inserts an item into a collection and returns it as an array
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="collection">The collection into which to insert the item.</param>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The modified collection as an array.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T[] Insert<T>(IEnumerable<T> collection, int index, T value)
        {
            var list = new List<T>(collection);

            if (index < 0 || index > list.Count)
                throw new ArgumentOutOfRangeException(nameof(index), string.Format(Lang.Get("Array.Index.0.{count}"), list.Count));

            list.Insert(index, value);
            return list.ToArray();
        }

        /// <summary>
        /// Removes an item from a collection and returns it as an array
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="collection">The collection from which to remove the item.</param>
        /// <param name="index">The index at which to remove the item.</param>
        /// <returns>The modified collection as an array.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T[] RemoveAt<T>(IEnumerable<T> collection, int index)
        {
            var list = new List<T>(collection);

            if (index < 0 || index > list.Count)
                throw new ArgumentOutOfRangeException(nameof(index), string.Format(Lang.Get("Array.Index.0.{count}"), list.Count));

            list.RemoveAt(index);
            return list.ToArray();
        }

        /// <summary>
        /// Fits a list to a specified length by adding or removing elements as necessary. 
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">The collection to fit.</param>
        /// <param name="length">The desired length of the resulting array.</param>
        /// <param name="value">The value to use when filling the array.</param>
        /// <param name="locale">The locale for localization.</param>
        /// <returns>The fitted collection as an array.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T[] Fit<T>(IEnumerable<T> collection, int length, T value = default, string locale = null)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), Lang.Get("Array.Length.Negative", locale: locale));

            if (length == collection.Count())
                return AsArray(collection);

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

            return list.ToArray();
        }

        private static Languages Lang { get; } = new Languages
        {
            { "Array.Length.Negative", "Array length cannot be negative." },
            { "Array.Index.0.{count}", "Index must be between 0 and {0}" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Array.Length.Negative", "La longitud del arreglo no puede ser negativa." },
                { "Array.Index.0.{count}", "El índice debe estar entre 0 y {0}." },
            }
        );
    }
}
