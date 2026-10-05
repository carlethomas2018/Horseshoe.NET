using System;
using System.Linq;
using Horseshoe.NET.Types;

namespace Horseshoe.NET.CodeTrace
{
    /// <summary>
    /// This class provides settings for the CodeTrace library. 
    /// </summary>
    public static class CodeTraceSettings
    {
        private static CodeTraceConfig Config;

        static CodeTraceSettings()
        {
            var subTypes = TypeUtil.GetSubTypes(typeof(CodeTraceConfig));
            if(subTypes.Any())
            {
                Config = (CodeTraceConfig)Activator.CreateInstance(subTypes.First());
            }
        }

        private static int? _CollectionStringMaxLength;

        /// <inheritdoc cref="CodeTraceConfig.CollectionStringMaxLength"/>
        public static int CollectionStringMaxLength => _CollectionStringMaxLength ?? Config.CollectionStringMaxLength;
    }
}
