using System;

namespace Horseshoe.NET.Expressions.Keywords
{
    public abstract class KeywordBase
    {
        public virtual string Name => GetType().Name;

        public virtual object Value { get; }

        public virtual Type ReturnType() => Value?.GetType() ?? typeof(object);
    }
}
