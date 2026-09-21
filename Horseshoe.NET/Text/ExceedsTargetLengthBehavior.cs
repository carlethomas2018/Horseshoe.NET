namespace Horseshoe.NET.Text
{
    /// <summary>
    /// What to do when a string exceeds the target length.
    /// </summary>
    public enum ExceedsTargetLengthBehavior
    {
        /// <summary>
        /// Keep the source string
        /// </summary>
        None,

        /// <summary>
        /// Throw an exception
        /// </summary>
        ThrowException,

        /// <summary>
        /// Truncate the string to the target length, possibly adding a truncate indicator
        /// </summary>
        Truncate
    }
}
