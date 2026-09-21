using Horseshoe.NET.Globalization;

namespace Horseshoe.NET
{
    /// <summary>
    /// Assertion methods for use across Horseshoe.NET and any client application.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Determines if two arguments are equal and throws an exception if they are not.
        /// </summary>
        /// <param name="description">An optional description of what is being asserted.</param>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <exception cref="AssertionFailedException"></exception>
        public static void Equals(string description, object arg1, object arg2)
        {
            description = description == null 
                ? "" 
                : description + (description.EndsWith(".") ? "" : ".") + " ";

            if (arg1 == null)
            {
                if (arg2 == null)
                    return;
                throw new AssertionFailedException(description + string.Format(Lang.Get("Assert.NotEquals"), "null", arg2));
            }
            else if (arg2 == null)
                throw new AssertionFailedException(description + string.Format(Lang.Get("Assert.NotEquals"), arg1, "null"));

            if (arg1.GetType() != arg2.GetType() && !arg1.GetType().IsAssignableFrom(arg2.GetType()) && !arg2.GetType().IsAssignableFrom(arg1.GetType()))
                throw new AssertionFailedException(description + Lang.Get("Assert.NotEquals.Type"));

            if (!object.Equals(arg1, arg2))
                throw new AssertionFailedException(description + string.Format(Lang.Get("Assert.NotEquals"), arg1, arg2));
        }

        /// <summary>
        /// Determines if two arguments are equal and throws an exception if they are not.
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <exception cref="AssertionFailedException"></exception>
        public static void Equals<T>(T arg1, T arg2)
        {
            Equals(null, arg1, arg2);
        }

        private static Languages Lang { get; } = new Languages
        {
            { "Assert.NotEquals", "Arguments are not equal: {0}, {1}" },
            { "Assert.NotEquals.Type", "Arguments are of different types and therefore are not equal" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Assert.NotEquals", "Los argumentos no son iguales" },
                { "Assert.NotEquals.Type", "Los argumentos son de diferentes tipos y, por lo tanto, no son iguales" },
            }
        );
    }
}
