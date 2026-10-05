using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

using Horseshoe.NET.Collections;
using Horseshoe.NET.DateAndTime;
using Horseshoe.NET.Globalization;

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
        /// <typeparam name="T">The type for which to get the default value</typeparam>
        /// <returns>The default value for the type</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T GetDefaultValue<T>() =>
            (T)GetDefaultValue(typeof(T));

        /// <summary>
        /// Gets the default value for a given type. For reference types, this is null; for value types, this is the default value (e.g., 0 for int, false for bool).
        /// </summary>
        /// <param name="type">The type for which to get the default value</param>
        /// <returns>The default value for the type</returns>
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

        /// <summary>
        /// Gets the subtypes of a given type.
        /// </summary>
        /// <param name="baseType">The base type for which to get subtypes.</param>
        /// <param name="includeBaseType">Indicates whether to include the base type in the results.</param>
        /// <param name="includeAbstractTypes">Indicates whether to include abstract types in the results.</param>
        /// <returns>An enumeration of subtypes.</returns>
        public static IEnumerable<Type> GetSubTypes(Type baseType, bool includeBaseType = false, bool includeAbstractTypes = false) =>
            GetTypes(t => baseType.IsAssignableFrom(t) && (includeBaseType || t != baseType) && (includeAbstractTypes || !t.IsAbstract));

        /// <summary>
        /// Gets all types from assemblies loaded in the current AppDomain that satisfy the supplied filter.
        /// </summary>
        /// <remarks>Scans assemblies returned by AppDomain.CurrentDomain.GetAssemblies(). Results are
        /// evaluated eagerly and returned as a list. Assembly.GetTypes may throw ReflectionTypeLoadException if some
        /// types cannot be loaded.</remarks>
        /// <param name="filter">A predicate that determines which types to include.</param>
        /// <returns>An IEnumerable<Type> containing types from all assemblies in the current AppDomain that match the predicate.</returns>
        public static IEnumerable<Type> GetTypes(Func<Type,bool> filter) =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(filter)
                .ToList();

        /// <summary>
        /// Attempts to get a type by its full name. 
        /// </summary>
        /// <param name="typeFullName">The fully qualified name of the type to find.</param>
        /// <param name="type">When this method returns, contains the type if found; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the type is found; otherwise, <c>false</c>.</returns>
        /// <exception cref="AmbiguousMatchException"></exception>
        public static bool TryGetType(string typeFullName, out Type type)
        {
            var types = ListUtil.AsList(GetTypes(t => t.FullName == typeFullName));

            switch(types.Count)
            {
                case 0:
                    type = null;
                    return false;
                case 1:
                    type = types[0];
                    return true;
                default:
                    throw new AmbiguousMatchException(string.Format(Lang.Get("AmbiguousMatch.{type}"), typeFullName));
            }
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
        public static object GetValue(object obj, string propertyName) =>
            GetValue(obj, propertyName, out _);

        /// <summary>
        /// Gets the value of a property of an object.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyType">On return, contains the type of the property if found; otherwise, <c>null</c>.</param>
        /// <returns>The value of the property</returns>
        /// <exception cref="AmbiguousMatchException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetValue(object obj, string propertyName, out Type propertyType)
        {
            propertyType = null;

            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (TryGetProperty(obj, propertyName, out PropertyInfo propertyInfo))
            {
                propertyType = propertyInfo.PropertyType;
                return propertyInfo.GetValue(obj);
            }

            throw new ArgumentException(string.Format(Lang.Get("PropertyNotFound.{prop}.{class}"), propertyName, obj.GetType().FullName));
        }

        /// <summary>
        /// Determines whether an object has a property with the specified name.
        /// </summary>
        /// <param name="obj">Object to inspect for the property.</param>
        /// <param name="propertyName">Name of the property to locate.</param>
        /// <returns><c>true</c> if the named property exists on the object; otherwise, <c>false</c>.</returns>
        public static bool HasProperty(object obj, string propertyName) =>
            TryGetProperty(obj, propertyName, out _);

        /// <summary>
        /// Attempts to retrieve the specified property of the provided object.
        /// </summary>
        /// <param name="obj">The object whose property is to be retrieved.</param>
        /// <param name="propertyName">The name of the property to locate.</param>
        /// <param name="propertyInfo">On return, contains the <see cref="PropertyInfo"/> for the named property if found; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="AmbiguousMatchException"></exception>
        /// <exception cref="ArgumentNullException">Thrown if obj is null.</exception>
        public static bool TryGetProperty(object obj, string propertyName, out PropertyInfo propertyInfo)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            propertyInfo = obj.GetType().GetProperty(propertyName);

            return propertyInfo != null;
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
        /// Determines whether the argument is a numeric type.
        /// </summary>
        /// <param name="type">A type</param>
        /// <returns><c>true</c> or <c>false</c></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNumeric(Type type)
        {
            if (type == null) 
                throw new ArgumentNullException(nameof(type));

            type = Nullable.GetUnderlyingType(type) ?? type;

            return
                type == typeof(sbyte)   || type == typeof(byte)   ||
                type == typeof(short)   || type == typeof(ushort) || 
                type == typeof(int)     || type == typeof(uint)   || 
                type == typeof(long)    || type == typeof(ulong)  || 
                type == typeof(float)   || 
                type == typeof(double)  ||
                type == typeof(decimal) ||
                type == typeof(BigInteger);
        }

        /// <summary>
        /// Gets the value of a property of an object, if applicable.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <returns><c>true</c> or <c>false</c></returns>
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
            var propertyInfo = obj.GetType().GetProperty(propertyName) ?? throw new ArgumentException(string.Format(Lang.Get("PropertyNotFound.{prop}.{class}"), propertyName, obj.GetType().FullName));
            propertyInfo.SetValue(obj, value);
        }

        private static Languages Lang { get; } = new Languages
        {
            { "PropertyNotFound.{prop}.{class}", "Property '{0}' not found in class '{1}'." },
            { "AmbiguousMatch.{type}", "Ambiguous match found for type '{0}'." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "PropertyNotFound.{prop}.{class}", "La propiedad '{0}' no se encontró en la clase '{1}'." },
                { "AmbiguousMatch.{type}", "Se encontró una coincidencia ambigua para el tipo '{0}'." },
            }
        );
    }
}
