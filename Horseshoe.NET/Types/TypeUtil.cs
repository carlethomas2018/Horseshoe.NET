using Horseshoe.NET.DateAndTime;
using Horseshoe.NET.Expressions;
using Horseshoe.NET.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horseshoe.NET.Types
{
    /// <summary>
    /// A collection of utility methods for working with types and reflection.
    /// </summary>
    public static class TypeUtil
    {
        /// <summary>
        /// Gets the default value for a given type. For reference types, this is null; for value types, this is the default value (e.g., 0 for int, false for bool).
        /// </summary>
        /// <param name="type">The type for which to get the default value</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetDefaultValue(Type type)
        {
            if (type == null) 
                throw new ArgumentNullException(nameof(type));

            if (type.IsValueType)
            {
                if (type == typeof(DateTime) && DateTimeConstants.PreferBusinessDates)
                    return DateTimeConstants.LowDate;

                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static IEnumerable<Type> GetSubTypes(Type baseType, bool includeAbstractTypes = false)
        {
            return GetTypes(t => baseType.IsAssignableFrom(t) && (includeAbstractTypes || !t.IsAbstract));
        }

        public static IEnumerable<Type> GetTypes(Func<Type,bool> filter)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(filter)
                .ToList();
        }

        /// <summary>
        /// Gets the value of a property of an object.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property</returns>
        /// <exception cref="AmbiguousMatchException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetValue(object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var propertyInfo = obj.GetType().GetProperty(propertyName) ?? throw new ArgumentException(string.Format(Lang.Get("Property.NotFound.{prop}.{class}"), propertyName, obj.GetType().FullName));

            return propertyInfo.GetValue(obj);
        }

        /// <summary>
        /// Detects whether a the argument is a Nullable<T> type.
        /// </summary>
        /// <param name="type">A type</param>
        /// <returns>true or false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNullable(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Determines whether a the argument is a numeric type.
        /// </summary>
        /// <param name="type">A type</param>
        /// <returns>true or false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNumeric(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return underlyingType == typeof(int) || underlyingType == typeof(long) || underlyingType == typeof(float) || underlyingType == typeof(double) || underlyingType == typeof(decimal);
        }

        /// <summary>
        /// Gets the value of a property of an object, if applicable.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <returns>true or false</returns>
        public static bool TryGetValue(object obj, string propertyName, out object value)
        {
            try
            {
                value = GetValue(obj, propertyName);
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Sets the value of a property of an object.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The new value of the property</param>
        /// <exception cref="AmbiguousMatchException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MethodAccessException"></exception>
        /// <exception cref="TargetException"></exception>
        /// <exception cref="TargetInvocationException"></exception>
        public static void SetValue(object obj, string propertyName, object value)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName) ?? throw new ArgumentException(string.Format(Lang.Get("Property.NotFound.{prop}.{class}"), propertyName, obj.GetType().FullName));
            propertyInfo.SetValue(obj, value);
        }

        private static Languages Lang { get; } = new Languages
        {
            { "Property.NotFound.{prop}.{class}", "Property '{0}' not found in class '{1}'." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Property.NotFound.{prop}.{class}", "La propiedad '{0}' no se encontró en la clase '{1}'." },
            }
        );
    }
}
