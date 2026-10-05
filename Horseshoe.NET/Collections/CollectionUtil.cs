using System;
using System.Collections.Generic;
using System.Linq;

namespace Horseshoe.NET.Collections
{
    /// <summary>
    /// A set of collection related utilities
    /// </summary>
    public static class CollectionUtil
    {
        /// <summary>
        /// Determines if a collection is not <c>null</c> and has any elements.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection</typeparam>
        /// <param name="collection">A collection</param>
        /// <returns><c>true</c> if collection is nither null nor empty</returns>
        public static bool HasAny<T>(IEnumerable<T> collection)
        {
            if (collection == null)
                return false;

            return collection.Any();
        }

        /// <summary>
        /// Determines if a collection is not <c>null</c> and has any elements that meet a specific condition.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection</typeparam>
        /// <param name="collection">A collection</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if collection is nither null nor devoid of items that meet a specific condition</returns>
        public static bool HasAny<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            if (collection == null)
                return false;

            return collection.Any(predicate);
        }

        public static bool IsNullOrEmpty<T>(IEnumerable<T> collection) => !HasAny(collection);

        /// <summary>
        /// Renders a collection of items to a single string e.g. <c>[ 1, "Bob", [null]]</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string ToString<T>(IEnumerable<T> collection)
        {
            if (collection == null)
                return "[null]";

            return collection.Any() 
                ? "{ " + string.Join(", ", collection.Select(t => Util.ToDisplayString(t))) + " }"
                : "{ }";
        }

        public static string ToCountAndLastString<T>(IEnumerable<T> collection)
        {
            if (collection == null)
                return "[null]";

            if (!collection.Any())
                return "{ }";

            return collection.Count() > 1
                ? "{[" + (collection.Count() - 1) + "]... " + Util.ToDisplayString(collection.Last()) + " }"
                : "{ " + Util.ToDisplayString(collection.Single()) + " }";
        }
    }
}
