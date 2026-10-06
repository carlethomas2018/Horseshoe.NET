namespace Horseshoe.NET
{
    public interface IConfigEngine
    {
        bool IsInited { get; }

        bool TryGetValue(string key, out string value);

        bool TryGetConnectionString(string key, out string connectionString);

        bool TryGetArray<T>(string path, out T[] array);

        bool TryGetInstance<T>(string path, out T instance) where T : class;
    }
}
