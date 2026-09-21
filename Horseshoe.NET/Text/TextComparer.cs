using System;
using System.Collections.Generic;

namespace Horseshoe.NET.Text
{
    public static class TextComparer
    {
        public class Char : IEqualityComparer<char>
        {
            public bool IgnoreCase { get; }

            public Char(bool ignoreCase = false)
            {
                IgnoreCase = ignoreCase;
            }

            public bool Equals(char x, char y) =>
                IgnoreCase
                    ? char.ToUpper(x) == char.ToUpper(y)
                    : x == y;

            public int GetHashCode(char c) =>
                c.GetHashCode();
        }

        public class String : IEqualityComparer<string>
        {
            public bool IgnoreCase { get; }

            public String(bool ignoreCase = false)
            {
                IgnoreCase = ignoreCase;
            }

            public bool Equals(string x, string y) =>
                IgnoreCase
                    ? string.Equals(x, y, StringComparison.OrdinalIgnoreCase)
                    : string.Equals(x, y);

            public int GetHashCode(string s) =>
                s.GetHashCode();
        }
    }
}
