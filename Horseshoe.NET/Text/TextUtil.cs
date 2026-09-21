using System;
using System.Globalization;
using System.Text;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Text
{
    public static class TextUtil
    {
        /// <summary>
        /// Applies the specified letter case to the given text, optionally using a specific locale for culture-specific casing rules.
        /// </summary>
        /// <param name="text">A text string</param>
        /// <param name="letterCase">The letter case to apply</param>
        /// <param name="locale">The locale for culture-specific casing rules (optional)</param>
        /// <returns>The text with the specified letter case</returns>
        public static string ApplyLetterCase(string text, LetterCase letterCase, string locale = null)
        {
            CultureInfo culture = locale == null
                ? CultureInfo.CurrentCulture
                : new CultureInfo(locale);

            switch (letterCase)
            {
                case LetterCase.Upper:
                    return text.ToUpper();
                case LetterCase.Lower:
                    return text.ToLower();
                case LetterCase.Title:
                    return culture.TextInfo.ToTitleCase(text);
                case LetterCase.Sentence:
                    var allLetters = text.ToCharArray();
                    bool isNewSentence = true;
                    for (int i = 0; i < allLetters.Length; i++)
                    {
                        char letter = allLetters[i];
                        if (isNewSentence && char.IsLetter(allLetters[i]))
                        {
                            allLetters[i] = char.ToUpper(letter);
                            isNewSentence = false;
                        }
                        if (letter == '.' || letter == '!' || letter == '?')
                        {
                            isNewSentence = true;
                        }
                    }
                    return new string(allLetters);
                case LetterCase.Camel:
                    return culture.TextInfo.ToTitleCase(text).Replace(" ", "");
                default:  // e.g. LetterCase.NotSpecified
                    return text;
            }
        }

        /// <summary>
        /// Adds padding to the left and right of the text to center it within a specified total width.  
        /// The default behavior is to throw an exception if the text exceeds the total width, but you can allow truncation by setting <paramref name="okToTruncate"/> to true.  
        /// If truncation occurs, a truncate indicator will be appended to the truncated text.
        /// </summary>
        /// <param name="text">A text string</param>
        /// <param name="totalWidth">The total width of the padded string</param>
        /// <param name="paddingChar">The character to use for padding</param>
        /// <param name="exceedsBehavior">The behavior to exhibit if the text exceeds the total width</param>
        /// <param name="truncateIndicator">The string to use as a truncate indicator</param>
        /// <param name="nudgeRightIfPaddingIsUneven">A value indicating whether to nudge the text to the right if the padding is uneven</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string PadCenter(string text, int totalWidth, char paddingChar = ' ', ExceedsTargetLengthBehavior exceedsBehavior = default, string truncateIndicator = "…", bool nudgeRightIfPaddingIsUneven = false)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Length > totalWidth)
            {
                switch (exceedsBehavior)
                {
                    case ExceedsTargetLengthBehavior.ThrowException:
                        throw new ArgumentException(Lang.Get("PadCenter.TotalWidth"), nameof(totalWidth));
                    case ExceedsTargetLengthBehavior.Truncate:
                        text = text.Substring(0, totalWidth - truncateIndicator.Length) + truncateIndicator;
                        break;
                    default:
                        return text;
                }
            }

            if (text.Length == totalWidth)
                return text;

            int paddingTotal = totalWidth - text.Length;
            int paddingLeft = nudgeRightIfPaddingIsUneven
                ? (int)Math.Ceiling(paddingTotal / 2.0)
                : paddingTotal / 2;
            int paddingRight = paddingTotal - paddingLeft;
            
            return string.Concat(new string(paddingChar, paddingLeft), text, new string(paddingChar, paddingRight));
        }

        /// <summary>
        /// Returns a string representation of the characters comprising the input text.  
        /// See also <see cref="RevealChar(char, string, string, string, string, string, string)"/>.
        /// </summary>
        /// <param name="text">The text to reveal.</param>
        /// <param name="blankSub">The string to substitute for blank strings.</param>
        /// <param name="nullSub">The string to substitute for null strings.</param>
        /// <param name="revealWhat">The types of characters to reveal.</param>
        /// <param name="spaceSub">The string to substitute for a space character.</param>
        /// <param name="nbSpaceSub">The string to substitute for a non-breaking space character.</param>
        /// <param name="tabSub">The string to substitute for a tab character.</param>
        /// <param name="crSub">The string to substitute for a carriage return character.</param>
        /// <param name="lfSub">The string to substitute for a line feed character.</param>
        /// <param name="crlfSub">The string to substitute for a carriage return + line feed sequence.</param>
        /// <param name="nonPrintableCharsSub">The string to substitute for non-printable characters.</param>
        /// <returns>The string representation of the text with specified characters revealed.</returns>
        public static string Reveal
        (
            string text,
            string blankSub = "[blank]",
            string nullSub = "[null]",
            RevealWhat revealWhat = RevealWhat.Whitespaces | RevealWhat.UnicodeChars | RevealWhat.AllNonPrintables,
            string spaceSub = "[space]",
            string nbSpaceSub = "[nbspace]",
            string tabSub = "[tab]",
            string crSub = "[cr]",
            string lfSub = "[lf]",
            string crlfSub = "[crlf]",
            string nonPrintableCharsSub = "[?]"
        )
        {
            if (text == null)
                return nullSub;

            if (text.Length == 0)
                return blankSub;

            if (text.Trim().Length == 0)
                revealWhat |= RevealWhat.Whitespaces;

            StringBuilder sb = new StringBuilder();

            foreach (char c in text.AsSpan())
            {
                sb.Append(RevealChar
                (
                    c, 
                    revealWhat, 
                    spaceSub, 
                    nbSpaceSub, 
                    tabSub, 
                    crSub, 
                    lfSub, 
                    nonPrintableCharsSub
                ));
            }

            text = sb.ToString();
            if ((revealWhat & RevealWhat.NewLines) == RevealWhat.NewLines)
            {
                text = text.Replace(crSub + lfSub, crlfSub);
            }

            return text;
        }

        /// <summary>
        /// Returning a string representation of a character, sometimes substituting specific characters with parameter-supplied strings or, 
        /// like in the case of control characters, a set of non-substitutable strings e.g. "[NUL]", "[SOH]", etc.
        /// </summary>
        /// <param name="c">The character to reveal.</param>
        /// <param name="spaceSub">The string to substitute for a space character.</param>
        /// <param name="nbSpaceSub">The string to substitute for a non-breaking space character.</param>
        /// <param name="tabSub">The string to substitute for a tab character.</param>
        /// <param name="crSub">The string to substitute for a carriage return character.</param>
        /// <param name="lfSub">The string to substitute for a line feed character.</param>
        /// <param name="nonPrintableCharsSub">The string to substitute for non-printable characters.</param>
        /// <returns>The string representation of the character.</returns>
        public static string RevealChar
        (
            char c,
            string spaceSub = "[space]",
            string nbSpaceSub = "[nbspace]",
            string tabSub = "[tab]",
            string crSub = "[cr]",
            string lfSub = "[lf]",
            string nonPrintableCharsSub = "[?]"
        )
        {
            return RevealChar
            (
                c, 
                RevealWhat.All, 
                spaceSub, 
                nbSpaceSub, 
                tabSub, 
                crSub, 
                lfSub, 
                nonPrintableCharsSub
            );
        }

        private static string RevealChar
        (
            char c,
            RevealWhat revealWhat,
            string spaceSub,
            string nbSpaceSub,
            string tabSub,
            string crSub,
            string lfSub,
            string nonPrintableCharsSub
        )
        {
            if (c < 128)
            {
                // ASCII 1 of 3 - Whitespace characters
                if ((revealWhat & RevealWhat.Spaces) == RevealWhat.Spaces && c == ' ')
                    return spaceSub;
                if ((revealWhat & RevealWhat.Tabs) == RevealWhat.Tabs && c == '\t')
                    return tabSub;
                if ((revealWhat & RevealWhat.NewLines) == RevealWhat.NewLines)
                {
                    if (c == '\r')
                        return crSub;
                    if (c == '\n')
                        return lfSub;
                }

                // ASCII 2 of 3 - Control characters
                if ((revealWhat & RevealWhat.AsciiControls) == RevealWhat.AsciiControls && char.IsControl(c))
                {
                    return c switch
                    {
                        '\x0000' => "[NUL]", // 0
                        '\x0001' => "[SOH]", // 1
                        '\x0002' => "[STX]", // 2
                        '\x0003' => "[ETX]", // 3
                        '\x0004' => "[EOT]", // 4
                        '\x0005' => "[ENQ]", // 5
                        '\x0006' => "[ACK]", // 6
                        '\x0007' => "[BEL]", // 7
                        '\x0008' => "[BS]",  // 8
                        //'\x0009':          // 9  - whitespace - tab
                        //'\x000A':          // 10 - whitespace - line feed
                        '\x000B' => "[VT]",  // 11
                        '\x000C' => "[FF]",  // 12
                        //'\x000D':          // 13 - whitespace - carriage return
                        '\x000E' => "[SO]",  // 14
                        '\x000F' => "[SI]",  // 15
                        '\x0010' => "[DLE]", // 16
                        '\x0011' => "[DC1]", // 17
                        '\x0012' => "[DC2]", // 18
                        '\x0013' => "[DC3]", // 19
                        '\x0014' => "[DC4]", // 20
                        '\x0015' => "[NAK]", // 21
                        '\x0016' => "[SYN]", // 22
                        '\x0017' => "[EDB]", // 23
                        '\x0018' => "[CAN]", // 24
                        '\x0019' => "[EM]",  // 25
                        '\x001A' => "[SUB]", // 26
                        '\x001B' => "[ESC]", // 27
                        '\x001C' => "[FS]",  // 28
                        '\x001D' => "[GS]",  // 29
                        '\x001E' => "[RS]",  // 30
                        '\x001F' => "[US]",  // 31
                        '\x007F' => "[DEL]", // 127
                        _ => $"[ctl {(int)c}]",
                    };
                }

                // ASCII 3 of 3 - Printable characters
                if
                (
                    (revealWhat & RevealWhat.AsciiLetters) == RevealWhat.AsciiLetters && char.IsLetter(c) ||
                    (revealWhat & RevealWhat.AsciiDigits) == RevealWhat.AsciiDigits && char.IsDigit(c) ||
                    (revealWhat & RevealWhat.AsciiPunctuation) == RevealWhat.AsciiPunctuation && char.IsPunctuation(c) ||
                    (revealWhat & RevealWhat.AsciiSymbols) == RevealWhat.AsciiSymbols && char.IsSymbol(c)
                )
                {
                    return $"[{c} {(int)c}]";
                }

                return new string(c, 1);
            }

            // Unicode 1 of 4 - Whitespace characters
            if ((revealWhat & RevealWhat.Whitespaces) == RevealWhat.Whitespaces && c == '\x00A0') // 160 - non-breaking space
                return nbSpaceSub;

            // Unicode 2 of 4 - Control characters
            if ((revealWhat & RevealWhat.UnicodeControls) == RevealWhat.UnicodeControls && char.IsControl(c))
            {
                return c switch
                {
                    '\x0080' => "[PAD]", // 128
                    '\x0081' => "[HOP]", // 129
                    '\x0082' => "[BPH]", // 130
                    '\x0083' => "[NBH]", // 131
                    '\x0084' => "[IND]", // 132
                    '\x0085' => "[NEL]", // 133
                    '\x0086' => "[SSA]", // 134
                    '\x0087' => "[ESA]", // 135
                    '\x0088' => "[HTS]", // 136
                    '\x0089' => "[HTJ]", // 137
                    '\x008A' => "[VTS]", // 138
                    '\x008B' => "[PLD]", // 139
                    '\x008C' => "[PLU]", // 140
                    '\x008D' => "[RI]",  // 141
                    '\x008E' => "[SS22]",// 142
                    '\x008F' => "[SS3]", // 143
                    '\x0090' => "[DCS]", // 144
                    '\x0091' => "[PU1]", // 145
                    '\x0092' => "[PU2]", // 146
                    '\x0093' => "[STS]", // 147
                    '\x0094' => "[CCH]", // 148
                    '\x0095' => "[MW]",  // 149
                    '\x0096' => "[SPA]", // 150
                    '\x0097' => "[EPA]", // 151
                    '\x0098' => "[SOS]", // 152
                    '\x0099' => "[SGCI]",// 153
                    '\x009A' => "[SCI]", // 154
                    '\x009B' => "[CSI]", // 155
                    '\x009C' => "[ST]",  // 156
                    '\x009D' => "[OSC]", // 157
                    '\x009E' => "[PM]",  // 158
                    '\x009F' => "[APC]", // 159
                    '\x061C' => "[ALM]", // 1564 arabic letter mark
                    _ => $"[ctl {(int)c}]",
                };
            }

            // Unicode 3 of 4 - Nonprintable characters
            if ((revealWhat & RevealWhat.NonPrintables) == RevealWhat.NonPrintables)
            {
                switch (c)
                {
                    case '\xFEFF':  // 65279
                        return "[byte-order-mark]";
                    case '\xFFFD':  // 65533
                        return nonPrintableCharsSub;
                }
            }

            // Unicode 4 of 4 - Printable characters
            if
            (
                (revealWhat & RevealWhat.UnicodeLetters) == RevealWhat.UnicodeLetters && char.IsLetter(c) ||
                (revealWhat & RevealWhat.UnicodeDigits) == RevealWhat.UnicodeDigits && char.IsDigit(c) ||
                (revealWhat & RevealWhat.UnicodePunctuation) == RevealWhat.UnicodePunctuation && char.IsPunctuation(c) ||
                (revealWhat & RevealWhat.UnicodeSymbols) == RevealWhat.UnicodeSymbols && char.IsSymbol(c)
            )
            {
                return $"[{c} {(int)c}u]";
            }

            return new string(c, 1);
        }

        /// <summary>
        /// Returns null if the input text is null, empty, or whitespace; otherwise, returns the trimmed text.
        /// </summary>
        /// <param name="text">A text string</param>
        /// <returns>The trimmed text or null if it is null, empty, or whitespace</returns>
        public static string Zap(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            text = text.Trim();

            if (string.IsNullOrEmpty(text))
                return null;

            return text;
        }

        private static Languages Lang { get; } = new Languages
        {
            { "PadCenter.TotalWidth", "Total width must be greater than or equal to the length of the text." }
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "PadCenter.TotalWidth", "El ancho total debe ser mayor o igual que la longitud del texto." }
            }
        );
    }
}
