using System;

namespace Horseshoe.NET.Text
{
    /// <summary>
    /// Enumeration that specifies which characters to reveal in method <see cref="TextUtil.Reveal(string, RevealWhat)"/>.
    /// </summary>
    [Flags]
    public enum RevealWhat
    {
        None = 0,
        Spaces = 1,
        Tabs = 2,
        NewLines = 4,
        Whitespaces = Spaces | Tabs | NewLines,
        AsciiLetters = 8,
        AsciiDigits = 16,
        AsciiPunctuation = 32,
        AsciiSymbols = 64,
        AsciiPunctuationAndSymbols = AsciiPunctuation | AsciiSymbols,
        AsciiChars = AsciiLetters | AsciiDigits | AsciiPunctuation | AsciiSymbols,
        UnicodeLetters = 128,
        UnicodeDigits = 256,
        UnicodePunctuation = 512,
        UnicodeSymbols = 1024,
        UnicodePunctuationAndSymbols = UnicodePunctuation | UnicodeSymbols,
        UnicodeChars = UnicodeLetters | UnicodeDigits | UnicodePunctuation | UnicodeSymbols,
        AllLetters = AsciiLetters | UnicodeLetters,
        AllPunctuation = AsciiPunctuation | UnicodePunctuation,
        AllSymbols = AsciiSymbols | UnicodeSymbols,
        AllPunctuationAndSymbols = AllPunctuation | AllSymbols,
        AllChars = AsciiChars | UnicodeChars,
        AsciiControls = 2048,
        UnicodeControls = 4096,
        AllControls = AsciiControls | UnicodeControls,
        NonPrintables = 8192,
        AllNonPrintables = AllControls | NonPrintables,
        All = Whitespaces | AllChars | AllNonPrintables
    }
}
