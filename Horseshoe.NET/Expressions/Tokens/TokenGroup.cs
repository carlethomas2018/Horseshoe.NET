using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Expressions.Tokens
{
    public abstract class TokenGroup : TokenBase, IValueToken
    {
        public abstract string MatchPattern { get; }
        private Regex _pattern;
        public Regex Pattern
        {
            get
            {
                _pattern ??= new Regex("^" + MatchPattern + "$");
                return _pattern;
            }
        }

        public IEnumerable<TokenBase> Tokens { get; }

        /// <inheritdoc cref="TokenBase.Type"/>
        public override TokenType Type => TokenType.Group;

        private object _returnValue;
        private Type _returnType;

        /// <inheritdoc cref="IValueToken.ReturnValue"/>
        public object ReturnValue 
        { 
            get 
            {
                if (_returnValue == null && _returnType == null)
                    _ProcessTokens();
                return _returnValue;
            }
        }

        /// <inheritdoc cref="IValueToken.ReturnType"/>
        public Type ReturnType 
        { 
            get 
            {
                if (_returnType == null)
                    _ProcessTokens();
                return _returnType;
            }
        }

        /// <inheritdoc cref="TokenBase.PatternIdentifier"/>
        public override string PatternIdentifier { get; }

        /// <summary>
        /// Constructor called via reflection by the expression engine
        /// </summary>
        public TokenGroup() : base()
        {
        }

        /// <summary>
        /// Constructor used by token group instances
        /// </summary>
        /// <param name="tokens">Tokens that have been grouped together</param>
        public TokenGroup(IEnumerable<TokenBase> tokens) : base
        (
            HasAny(tokens) 
                ? string.Join("", tokens.Select(t => t.RawValue)) 
                : throw new ExpressionException(Lang.Get("Token.Group.Initialize")), 
            tokenPos: tokens.First().TokenPos
        )
        {
            Tokens = tokens;
            PatternIdentifier = string.Join("", tokens.Select(t => t.RawValue));
        }

        public void IsMatch(string groupPattern) =>
            Pattern.IsMatch(groupPattern);

        private void _ProcessTokens()
        {
            ProcessTokens(out object returnValue, out Type returnType);
            _returnValue = returnValue;
            _returnType = returnType;
        }

        public abstract void ProcessTokens(out object returnValue, out Type returnType);

        private static Languages Lang { get; } = new Languages
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
