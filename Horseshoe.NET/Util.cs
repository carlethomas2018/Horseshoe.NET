using System;
using System.Collections;
using System.Reflection;

using Horseshoe.NET.Collections;
using Horseshoe.NET.Types;

namespace Horseshoe.NET
{
    /// <summary>
    /// Utility methods for Horseshoe.NET
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Displays objects in a uniform, programming language adjacent way e.g. numbers and bools... <c>-3.1, 3.141592652589, true</c>; others... <c>"Hello world!", "2/12/2022", "System.String", "ctor:MyClass"</c>
        /// </summary>
        /// <param name="obj">An object</param>
        /// <param name="fqn">If <c>true</c> and <c>obj</c> is a type, returns the fully qualified type name, e.g. <c>MyNamespace.MyClass</c>. Default is <c>false</c> e.g. <c>MyClass</c>.</param>
        /// <returns>A display string</returns>
        public static string ToDisplayString(object obj, bool fqn = false)
        {
            if (obj == null)
                return "[null]";

            Type type = obj.GetType();
            type = Nullable.GetUnderlyingType(type) ?? type;  // unwrap nullable

            if (type.IsNumeric() || type == typeof(bool))
                return obj.ToString();

            if (obj is IEnumerable collection)
                return CollectionUtil.Render(collection);

            if (obj is DateTime dateTime)
            {
                obj = dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0
                    ? dateTime.ToShortDateString()
                    : dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            }

            else if (obj is MethodBase methodBase)
            {
                obj = methodBase.IsConstructor
                    ? "ctor:" + (fqn ? methodBase.DeclaringType.FullName : methodBase.DeclaringType.Name)
                    : (fqn ? methodBase.DeclaringType.FullName : methodBase.DeclaringType.Name) + "." + methodBase.Name;
            }

            else  // includes enums
            {
                obj = obj.ToString();
            }
            return "\"" + obj + "\"";
        }
    }
}
