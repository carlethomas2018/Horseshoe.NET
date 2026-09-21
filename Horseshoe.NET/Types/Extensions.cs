using System;
using System.Linq;
using System.Reflection;

namespace Horseshoe.NET.Types
{
    /// <summary>
    /// A collection of extension methods for working with types and reflection.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the names of the properties of a type.
        /// </summary>
        /// <param name="type">A type</param>
        /// <param name="bindingFlags">Optional binding flags (default is Public | Instance).</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string[] GetPropertyNames(this Type type, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            if (type == null) 
                throw new ArgumentNullException(nameof(type));

            return type.GetProperties(bindingFlags)
                .Select(p => p.Name)
                .ToArray();
        }

        /// <inheritdoc cref="TypeUtil.GetValue(object, string)"/>
        public static object GetValue(this object obj, string propertyName)
        {
            return TypeUtil.GetValue(obj, propertyName);
        }

        /// <inheritdoc cref="TypeUtil.IsNullable(Type)"/>
        public static bool IsNullable(this Type type)
        {
            return TypeUtil.IsNullable(type);
        }

        /// <inheritdoc cref="TypeUtil.IsNumeric(Type)"/>
        public static bool IsNumeric(this Type type)
        {
            return TypeUtil.IsNumeric(type);
        }

        /// <inheritdoc cref="TypeUtil.SetValue(object, string, object)"/>
        public static void SetValue(this object obj, string propertyName, object value)
        {
            TypeUtil.SetValue(obj, propertyName, value);
        }

        /// <inheritdoc cref="TypeUtil.TryGetValue(object, string, out object)"/>
        public static bool TryGetValue(this object obj, string propertyName, out object value)
        {
            return TypeUtil.TryGetValue(obj, propertyName, out value);
        }

        public static string ToShortName(this Type type, bool shortenFullNames = false)
        {
            if (type == typeof(string))
                return "string";
            if (type == typeof(DateTime)) 
                return "datetime";
            if (type == typeof(bool))
                return "bool";
            if (type == typeof(short))
                return "short";
            if (type == typeof(int))
                return "int";
            if (type == typeof(long))
                return "long";
            if (type == typeof(float))
                return "float";
            if (type == typeof(double))
                return "double";
            if (type == typeof(decimal))
                return "decimal";

            return shortenFullNames
                ? type.Name
                : type.FullName;
        }
    }
}
