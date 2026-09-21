using System;
using System.Linq;
using System.Reflection;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Expressions.Functions
{
    /// <summary>
    /// Function classes must extend <c>FunctionBase</c> to enable function parsing and evaluation.
    /// Each must contain exactly one 'Execute' method with a unique signature to avoid accidental overrides.
    /// </summary>
    public abstract class FunctionBase
    {
        public virtual string Name => GetType().Name;

        private MethodInfo executeMethod;
        internal MethodInfo ExecuteMethod
        {
            get
            {
                if (executeMethod == null)
                    executeMethod = 
                        GetType().GetMethod("Execute") 
                        ?? throw new ExpressionException(string.Format(Lang.Get("Function.NoExecute.{name}"), Name));
                return executeMethod;
            }
        }

        private Type returnType;
        public Type ReturnType 
        { 
            get => returnType ?? ExecuteMethod.ReturnType;
            protected set
            {
                returnType = value;
            } 
        }

        public int MinArgs => ExecuteMethod.GetParameters().Count(p => !p.IsOptional);

        public int MaxArgs => ExecuteMethod.GetParameters().Count();

        protected static Languages Lang { get; } = new Languages
        {
            { "Function.NoExecute.{name}", "This function class must contain one method named 'Execute': {0}" },
            { "Function.Mismatch.{type1}.{type2}", "Function arg type mismatch, expected '{0}', got '{1}'" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Function.NoExecute.{name}", "Esta clase de función debe contener un método llamado 'Execute': {0}" },
                { "Function.Mismatch.{type1}.{type2}", "Tipo de argumento de función no coincide, se esperaba '{0}', se recibió '{1}'" },
            }
        );
    }
}
