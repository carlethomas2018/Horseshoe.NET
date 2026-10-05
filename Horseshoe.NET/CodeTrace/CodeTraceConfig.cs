namespace Horseshoe.NET.CodeTrace
{
    /// <summary>
    /// This class provides overridable configuration settings for the CodeTrace library. 
    /// </summary>
    public abstract class CodeTraceConfig
    {
        /// <summary>
        /// The default maximum length of a string representation of a collection (e.g., array, list, dictionary) when logged by CodeTrace.
        /// </summary>
        public static int DefaultCollectionStringMaxLength { get; } = 100;

        /// <summary>
        /// The maximum length of a string representation of a collection (e.g., array, list, dictionary) when logged by CodeTrace.
        /// </summary>
        public abstract int CollectionStringMaxLength { get; }
    }
}
