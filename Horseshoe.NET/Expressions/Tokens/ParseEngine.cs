using System;
using System.Collections.Generic;
using System.Linq;
using Horseshoe.NET.Globalization;
using Horseshoe.NET.Types;

namespace Horseshoe.NET.Expressions.Tokens
{
    public class ParseEngine
    {
        public List<TokenBase> Tokens { get; } = new List<TokenBase>();
        public List<TokenBase> Parsers { get; }

        public ParseEngine()
        {
            Parsers = TypeUtil.GetSubTypes(typeof(TokenBase))
                .Select(t => (TokenBase)Activator.CreateInstance(t))
                .OrderBy(p => p.Priority)
                .ToList();
        }

        public void Start(string rawSource)
        {
            int pos = 0;
            int lastPos = -1;
            var span = rawSource.AsSpan();

            while (pos < span.Length - 1)
            {
                if (lastPos == pos)
                    throw new ExpressionException(string.Format(Lang.Get("Token.NotAdvanced.{pos}"), pos));

                lastPos = pos;

                foreach (var parser in Parsers)
                {
                    if (parser.Parse(span, ref pos, Tokens.ToArray(), out string rawValue, out int startPos))
                    {
                        Tokens.Add(parser.CreateInstance(rawValue, startPos));

                    }
                    throw new ExpressionException(Lang.Get("Token.NotFound"));
                }
            }
        }

        public string ReconstructInput()
        {
            return string.Join("", Tokens.Select(t => t.RawValue));
        }

        private static Languages Lang { get; } = new Languages
        {
            { "Token.Unexpected.{char}.{index}", "Unexpected char '{0}': {1}" },
            { "Token.Unrecognized.{value}.{index}", "Unrecognized token '{0}': {1}" },
            { "Token.MissingExpected.{value}.{index}", "Missing expected char '{0}': {1}" },
            { "Token.NotFound", "No token was found in source text" },
            { "Token.NotAdvanced.{pos}", "Parsing has not advanced the cursor position: {0}" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Token.Unexpected.{char}.{index}", "Carácter inesperado '{0}': {1}" },
                { "Token.Unrecognized.{value}.{index}", "Token no reconocido '{0}': {1}" },
                { "Token.MissingExpected.{char}.{index}", "Falta un carácter esperado '{0}': {1}" },
                { "Token.NotFound", "No se encontró ningún token en el texto fuente" },
                { "Token.NotAdvanced.{pos}", "El análisis no ha avanzado la posición del cursor: {0}" },
            }
        );
    }
}
