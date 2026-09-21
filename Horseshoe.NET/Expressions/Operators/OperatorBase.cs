using System;
using System.Reflection;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Expressions.Operators
{
    /// <summary>
    /// Operator classes must extend <c>OperatorBase</c> to enable function parsing and evaluation.
    /// Each must contain exactly one 'Execute' method with 2 args.
    /// </summary>
    public abstract class OperatorBase
    {
        public virtual string Name => GetType().Name;

        public virtual string[] Symbols { get; }

        private MethodInfo executeMethod;
        internal MethodInfo ExecuteMethod
        {
            get
            {
                if (executeMethod == null)
                {
                    executeMethod =
                        GetType().GetMethod("Execute")
                        ?? throw new ExpressionException(string.Format(Lang.Get("Operator.NoExecute.{name}"), Name));

                    Assert.Equals(Lang.Get("Operator.Assert.Param.2"), executeMethod.GetParameters().Length, 2);
                }
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

        protected static Languages Lang { get; } = new Languages
        {
            { "Operator.NoExecute.{name}", "This operator class must contain one method named 'Execute': {0}" },
            { "Operator.Mismatch.{type1}.{type2}", "Operator arg type mismatch, expected '{0}', got '{1}'" },
            { "Operator.Assert.Param.2", "Operator args must equal 2" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Operator.NoExecute.{name}", "Esta clase de operador debe contener un método llamado 'Execute': {0}" },
                { "Operator.Mismatch.{type1}.{type2}", "Tipo de argumento de operador no coincide, se esperaba '{0}', se recibió '{1}'" },
                { "Operator.Assert.Param.2", "Los argumentos del operador deben ser 2" },
            }
        );
    }
}
