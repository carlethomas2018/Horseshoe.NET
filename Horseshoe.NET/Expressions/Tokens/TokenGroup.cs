using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Horseshoe.NET.Collections;
using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Expressions.Tokens
{
    public class TokenGroup : TokenBase, IValueToken
    {
        private static Regex _arithmeticPattern;
        private static Regex ArithmeticPattern 
        {
            get 
            {
                _arithmeticPattern ??= new Regex("^N[BCFKPS](OMN[BCFKPS])+$"); // '1 + 1', '1 - 1 * 1 + 1', etc.
                return _arithmeticPattern;
            }
        }

        public IEnumerable<TokenBase> Tokens { get; }

        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Group;

        /// <inheritdoc cref="IValueToken.Value"/>
        public object Value => GetValue();

        /// <inheritdoc cref="IValueToken.ValueType"/>
        public Type ValueType => typeof(string);

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override string PatternIdentifier { get; }

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        public TokenGroup() : base() 
        { 
        }

        /// <summary>
        /// Constructor used by token instances, also called via reflection by the parse engine
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public TokenGroup(IEnumerable<TokenBase> tokens) : base(CollectionUtil.HasAny(tokens) 
                                                                    ? string.Join("", tokens.Select(t => t.RawValue)) 
                                                                    : throw new ExpressionException(Lang.Get("Token.Group.Initialize")), 
                                                                tokenPos: tokens.First().TokenPos)
        {
            Tokens = tokens;
            PatternIdentifier = string.Join("", tokens.Select(t => t.RawValue));
        }

        private object GetValue()
        {
            
        }

        protected static Languages Lang { get; } = new Languages
        {
            { "Token.Group.Initialize", "Cannot initialize token group, no tokens." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Token.Group.Initialize", "No se pudo iniciar el grupo de tokens, no hay tokens." },
            }
        );
    }
}
