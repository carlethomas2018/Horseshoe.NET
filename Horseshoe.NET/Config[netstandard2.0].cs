using Microsoft.Extensions.Configuration;

using Horseshoe.NET.Types;

namespace Horseshoe.NET
{
    public class Config
    {
        public IConfiguration Configuration { get; set; }

        public bool IsValid => Configuration != null;

        public bool TryGetValue(string key, out string value)
        {
            value = Configuration[key];
            return value != null;
        }

        public bool TryGetConnectionString(string key, out string connectionString)
        {
            connectionString = Configuration.GetConnectionString(key);
            return connectionString != null;
        }

        public bool TryGetArray<T>(string path, out T[] array)
        {
            array = null;
            var section = Configuration.GetSection(path);
            if (section.Exists())
            {
                array = section.Get<T[]>();
                return true;
            }
            return false;
        }

        public bool TryGetInstance<T>(string path, out T instance) where T : class
        {
            instance = null;
            var section = Configuration.GetSection(path);
            if (section.Exists())
            {
                instance = TypeUtil.GetDefaultValue<T>();
                section.Bind(instance);
                return true;
            }
            return false;
        }
    }
}
