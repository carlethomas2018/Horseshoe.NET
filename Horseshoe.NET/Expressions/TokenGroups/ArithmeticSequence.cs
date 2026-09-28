using System;
using System.Collections.Generic;

using Horseshoe.NET.Collections;
using Horseshoe.NET.Expressions.Tokens;
using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Expressions.TokenGroups
{
    public class ArithmeticSequence : TokenGroup
    {
        public override string MatchPattern => "N[BCFKPS]([%]NB|([-+×÷]N[BCFKPS])+)";  // e.g. "10 % 2", "1 + 2", "1 + 2 * 3 - 4"

        /// <summary>
        /// Constructor called via reflection by the expression engine
        /// </summary>
        internal ArithmeticSequence()
        {
        }

        /// <summary>
        /// Constructor used by token group instances
        /// </summary>
        /// <param name="tokens">Tokens that have been grouped together</param>
        public ArithmeticSequence(IEnumerable<TokenBase> tokens) : base(tokens)
        {
        }

        public override void ProcessTokens(out object returnValue, out Type returnType)
        {
            returnType = typeof(double);

            var list = ListUtil.AsList(Tokens);
            TempNumber tempNumber;

            if (list.Count % 2 == 0)
                throw new ThisShouldNeverHappenException("Expected odd number of tokens");

            while (list.Count > 3)
            {
                for (int i = 1; i < list.Count; i += 2)
                {
                    if (list[i] is Operator oper)
                    {
                        if (oper.OperatorType.In(OperatorType.Multiply, OperatorType.Divide))  // P.E. -> M.D. <- A.S
                        {
                            tempNumber = new TempNumber(ProcessSingleArithmeticSequence(list[i - 1] as NumericBase, list[i] as Operator, list[i + 1] as NumericBase), list[i - 1].TokenPos);

                            list.Insert(i - 1, tempNumber);
                            list.RemoveAt(i);
                            list.RemoveAt(i);
                            list.RemoveAt(i);
                            break;
                        }
                    }
                    else throw new ThisShouldNeverHappenException("Expected operator");
                }

                tempNumber = new TempNumber(ProcessSingleArithmeticSequence(list[0] as NumericBase, list[1] as Operator, list[2] as NumericBase), list[0].TokenPos);

                list.Insert(0, tempNumber);
                list.RemoveAt(1);
                list.RemoveAt(1);
                list.RemoveAt(1);
                break;

            }
            returnValue = ProcessSingleArithmeticSequence(list[0] as NumericBase, list[1] as Operator, list[2] as NumericBase);
        }

        public static double ProcessSingleArithmeticSequence(NumericBase number1, Operator operator1, NumericBase number2)
        {
            if (number1 == null)
                throw new ExpressionException(string.Format(Lang.Get("Token.Group.ArithmeticSequence.Invalid.{tokenName}"), nameof(number1)));
            if (operator1 == null)
                throw new ExpressionException(string.Format(Lang.Get("Token.Group.ArithmeticSequence.Invalid.{tokenName}"), nameof(operator1)));
            if (number2 == null)
                throw new ExpressionException(string.Format(Lang.Get("Token.Group.ArithmeticSequence.Invalid.{tokenName}"), nameof(number2)));

            return operator1.OperatorType switch
            {
                OperatorType.Add =>      (double)number1.ReturnValue + (double)number2.ReturnValue,
                OperatorType.Subtract => (double)number1.ReturnValue - (double)number2.ReturnValue,
                OperatorType.Multiply => (double)number1.ReturnValue * (double)number2.ReturnValue,
                OperatorType.Divide =>   (double)number1.ReturnValue / (double)number2.ReturnValue,
                OperatorType.Modulus =>  (double)number1.ReturnValue % (double)number2.ReturnValue,
                _ => throw new ThisShouldNeverHappenException("unexpected operator type"),
            };
        }

        private static Languages Lang { get; } = new Languages
        {
            { "Token.Group.ArithmeticSequence.Invalid.{tokenName}", "Cannot initialize token group, invalid token in arithmetic sequence: {0}" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Token.Group.ArithmeticSequence.Invalid.{tokenName}", "No se pudo iniciar el grupo de tokens, token inválido en la secuencia aritmética: {0}" },
            }
        );
    }
}
