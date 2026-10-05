using System.Globalization;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET
{
    public static class SystemSettings
    {
        public static Config Config { get; } = new Config();

        /// <summary>
        /// Gets a configuration value by key. If a required key is not found, an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">The type of the configuration value to retrieve.</typeparam>
        /// <param name="key">The key for the configuration value to retrieve.</param>
        /// <param name="required">Indicates whether the key is required.</param>
        /// <param name="defaultValue">An optional default value to return if the key is not found.</param>
        /// <param name="locale">An optional locale to use for parsing date values and numbers.</param>
        /// <param name="numberStyle">An optional number style to use for parsing numbers.</param>
        /// <param name="dateFormat">An optional date format to use for parsing date values.</param>
        /// <param name="dateTimeStyle">An optional date time style to use for parsing date values.</param>
        /// <param name="additionalData">An optional object containing additional data to use for parsing.</param>
        /// <returns>The configuration value.</returns>
        /// <exception cref="ConfigException"></exception>
        public static T GetConfigValue<T>(string key, bool required = true, T defaultValue = default, string locale = null, NumberStyles numberStyle = NumberStyles.None, string dateFormat = null, DateTimeStyles dateTimeStyle = DateTimeStyles.None, object additionalData = null)
        {
            //// Handle special case for default strings
            //if (typeof(T) == typeof(string) && defaultValue == null)
            //    defaultValue = (T)(object)string.Empty;

            if (string.IsNullOrWhiteSpace(key))
                throw new ConfigException(Lang.Get("InvalidKey"));

            if (!Config.IsValid)
            {
                if (required)
                    throw new ConfigException(Lang.Get("InvalidConfig"));

                return defaultValue;
            }

            // Handle special case for hex numbers
            if (key != null && key.EndsWith("[hex]"))
            {
                key = key.Substring(0, key.Length - 5);
                numberStyle |= NumberStyles.HexNumber;
            }

            // Get and parse the value from the configuration
            if (Config.TryGetValue(key, out string value))
            {
                if (required && string.IsNullOrWhiteSpace(value))
                    throw new ConfigException(string.Format(Lang.Get("RequiredValueNotFound"), key));

                return Parse.Value<T>(value, defaultValue: defaultValue, locale: locale, numberStyle: numberStyle, dateFormat, dateTimeStyle, additionalData, strict: true);
            }

            if (required)
                throw new ConfigException(string.Format(Lang.Get("RequiredKeyNotFound"), key));

            return defaultValue;
        }

        /// <summary>
        /// Gets a connection string by key from the configuration.
        /// </summary>
        /// <param name="key">The connection string key</param>
        /// <param name="required">Indicates whether the connection string is required</param>
        /// <returns>The connection string, or null if not found and not required</returns>
        /// <exception cref="ConfigException"></exception>
        public static string GetConnectionString(string key, bool required = true)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ConfigException(Lang.Get("InvalidKey"));

            if (!Config.IsValid)
            {
                if (required)
                    throw new ConfigException(Lang.Get("InvalidConfig"));

                return null;
            }

            if (Config.TryGetConnectionString(key, out string connectionString))
            {
                if (required && string.IsNullOrWhiteSpace(connectionString))
                    throw new ConfigException(string.Format(Lang.Get("RequiredValueNotFound"), key));

                return connectionString;
            }

            if (required)
                throw new ConfigException(string.Format(Lang.Get("RequiredKeyNotFound"), key));

            return null;
        }

        /// <summary>
        /// Gets an array of configuration values by path. If a required path is not found, an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array</typeparam>
        /// <param name="path">The path to the array in the configuration</param>
        /// <param name="required">Indicates whether the array is required</param>
        /// <returns>The array of configuration values, or null if not found and not required</returns>
        /// <exception cref="ConfigException"></exception>
        public static T[] GetArray<T>(string path, bool required = true)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ConfigException(Lang.Get("InvalidKey"));

            if (!Config.IsValid)
            {
                if (required)
                    throw new ConfigException(Lang.Get("InvalidConfig"));

                return null;
            }

            if (Config.TryGetArray<T>(path, out T[] array))
            {
                if (required && (array == null || array.Length == 0))
                    throw new ConfigException(string.Format(Lang.Get("RequiredValueNotFound"), path));

                return array;
            }
            if (required)
                throw new ConfigException(string.Format(Lang.Get("RequiredKeyNotFound"), path));

            return null;
        }

        private static Languages Lang { get; } = new Languages
        {
            { "InvalidKey", "The provided key / path is invalid." },
            { "InvalidConfig", "SystemSettings has not been initialized with a valid Config instance." },
            { "RequiredKeyNotFound", "Required key '{0}' not found in configuration." },
            { "RequiredValueNotFound", "Required value for key '{0}' not found in configuration." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "InvalidKey", "La clave / ruta proporcionada es inválida." },
                { "InvalidConfig", "SystemSettings no ha sido inicializado con una instancia de Config válida." },
                { "RequiredKeyNotFound", "Clave requerida '{0}' no encontrada en la configuración." },
                { "RequiredValueNotFound", "Valor requerido para la clave '{0}' no encontrado en la configuración." },
            }
        );
    }
}
