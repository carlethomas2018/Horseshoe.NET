using System;
using System.Diagnostics;
using Horseshoe.NET.Data;
using Horseshoe.NET.DateAndTime;
using Horseshoe.NET.Expressions;
using Horseshoe.NET.Expressions.Tokens;
using Horseshoe.NET.Globalization;
using Horseshoe.NET.Text;
using Horseshoe.NET.Types;

namespace Horseshoe.NET.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TestLanguage();
            //TestTimeSpanRendering();
            //TestReveal();
            //TestPad();
            //TestDataGrid();
            TestExpressions();
        }

        static readonly Languages Lang = new Languages()
        {
            {  "Msg.Hello", "Hello" }
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Msg.Hello", "Hola" }
            },
            new Language("fr")
            {
                { "Msg.Hello", "Bonjour" }
            }
        );

        static void TestLanguage()
        {
            Console.WriteLine(Lang.Get("Msg.Hello"));          // Output: Hello (assuming default locale is "en-US")
            Console.WriteLine(Lang.Get("Msg.Hello", "en-US")); // Output: Hello (locale = "en-US")
            Console.WriteLine(Lang.Get("Msg.Hello", "es-ES")); // Output: Hola (locale = "es-ES")
            Console.WriteLine(Lang.Get("Msg.Hello", "fr-FR")); // Output: Bonjour (locale = "fr-FR")
            Console.WriteLine(Lang.Get("Msg.Hello", "de-DE")); // Output: Hello (locale = "de-DE", falling back to "en-US")
            Console.WriteLine(Lang.Get("Msg.Hello2"));         // Output: Msg.Hello2 (fallback to key)
        }

        static void TestTimeSpanRendering()
        {
            var dateOfBirth = new System.DateTime(1979, 6, 27);
            Console.WriteLine("Date of birth: {0}", dateOfBirth);
            Console.WriteLine("  Age: {0} (no time)", dateOfBirth.Age(ignoreTime: true));
            Console.WriteLine("  Age in years: {0:#.#}", dateOfBirth.AgeInYears(ignoreTime: true));
            Console.WriteLine("  Age in months: {0:#.#}", dateOfBirth.AgeInMonths(ignoreTime: true));
            Console.WriteLine("  Age in weeks: {0:#.#}", dateOfBirth.AgeInWeeks(ignoreTime: true));
            Console.WriteLine("  Age in days: {0:#.#}", dateOfBirth.AgeInDays(ignoreTime: true));
            Console.WriteLine("  Age in hours: {0:#.#}", dateOfBirth.AgeInHours(ignoreTime: true));
            Console.WriteLine("  Age in minutes: {0:#.#}", dateOfBirth.AgeInMinutes(ignoreTime: true));
            Console.WriteLine("  Age in seconds: {0:#.#}", dateOfBirth.AgeInSeconds(ignoreTime: true));
            Console.WriteLine("  Age in milliseconds: {0}", dateOfBirth.AgeInMilliseconds(ignoreTime: true));
            var date = new System.DateTime(1990, 1, 1);
            var span = date.Age();
            Console.WriteLine("Date: {0}", date);
            Console.WriteLine("  Span: {0}", span);
            Console.WriteLine("  Years: {0}", span.ExtYears());
            Console.WriteLine("  Months: {0}", span.ExtMonths());
            Console.WriteLine("  Weeks: {0}", span.ExtWeeks());
            Console.WriteLine("  Days: {0}", span.ExtDays());
            Console.WriteLine("  (Original Days: {0})", span.Days);
            Console.WriteLine("  Hours: {0}", span.Hours);
            Console.WriteLine("  Minutes: {0}", span.Minutes);
            Console.WriteLine("  Seconds: {0}", span.Seconds);
            Console.WriteLine("  Milliseconds: {0}", span.Milliseconds);
            Console.WriteLine("  Rendered (all): {0}", span.ExtToString(renderHint: TimeSpanRenderHint.All));
            Console.WriteLine("  Rendered (all non-zero, \"es-ES\"): {0}", span.ExtToString(locale: "es-ES", renderHint: TimeSpanRenderHint.All, nonZeroOnly: true));
            Console.WriteLine("  Rendered (2): {0}", span.ExtToString(renderHint: TimeSpanRenderHint.RuleOfTwo));
            Console.WriteLine("  Rendered (3): {0}", span.ExtToString(renderHint: TimeSpanRenderHint.RuleOfThree));
            Console.WriteLine("  Rendered (4): {0}", span.ExtToString(renderHint: TimeSpanRenderHint.RuleOfFour));
        }

        static void TestReveal()
        {
            var text = "Çhïćķêŉ";
            Console.WriteLine("Original text:     " + text);
            Console.WriteLine("Letters Revealed:  " + TextUtil.Reveal(text, revealWhat: RevealWhat.AllLetters));
            text = "\0\x0001\x0019";
            Console.WriteLine("Original text:     " + text);
            Console.WriteLine("Controls Revealed: " + TextUtil.Reveal(text, revealWhat: RevealWhat.AsciiControls));
            text = "*#$%^&*";
            Console.WriteLine("Original text:     " + text);
            Console.WriteLine("Symbols Revealed:  " + TextUtil.Reveal(text, revealWhat: RevealWhat.AsciiPunctuationAndSymbols));
        }

        static void TestPad()
        {
            int width = 32;
            Console.WriteLine("The quick brown fox".PadRight(width) + ".");
            Console.WriteLine(TextUtil.PadCenter("The quick brown fox", width) + ".");
            Console.WriteLine(TextUtil.PadCenter("The quick brown fox", width, nudgeRightIfPaddingIsUneven: true) + ".");
            Console.WriteLine("The quick brown fox".PadLeft(width) + ".");
        }

        static void TestDataGrid()
        {
            DateTimeConstants.PreferBusinessDates = true;

            DataGrid grid;
            try
            {
                grid = new DataGrid("Test Grid")
                    .AddColumn<string>("Name")
                    .AddColumn<int>("Age")
                    .AddRow("Alice", 30)
                    .AddRow("Bob", 25)
                    .AddRow("Charlie", "thirty-five");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Render());
                Console.WriteLine();
            }

            grid = new DataGrid("Test Grid")
                .AddColumn<string>("Name")
                .AddColumn<int>("Age")
                .AddColumn<DateTime>("Date of Hire", format: "yyyy-MM-dd")
                .AddColumn<string>("Favorite Snack", nullFormat: "N/A")
                .AddRow("Alice", 30, new DateTime(2009, 07, 25))
                .AddRow("Bob", 25, new DateTime(2010, 01, 15), "Popcorn")
                .AddRow("Charlie", 35, new DateTime(2008, 11, 30), "Ice Cream");

            grid.AddRow();
            grid.String["Name"] = "Diana";
            grid.Int["Age"] = 28;
            grid.Row = 0;

            Console.WriteLine
            (
                grid.RenderGrid()
            );
            Console.WriteLine
            (
                grid.RenderGrid
                (
                    new DataGridRenderingHints
                    {
                        ShowColumnHeaderRow = true,
                        InnerHorizontalBorders = true,
                        InnerVerticalBorders = true
                    }
                )
            );
            Console.WriteLine
            (
                grid.RenderGrid
                (
                    new DataGridRenderingHints
                    {
                        ShowColumnHeaderRow = true,
                        HighlightCurrentRow = true,
                        InnerHorizontalBorders = true,
                        InnerVerticalBorders = true,
                        OuterHorizontalBorders = true,
                        OuterVerticalBorders = true
                    }
                )
            );
            Console.WriteLine
            (
                grid.RenderGrid
                (
                    new DataGridRenderingHints
                    {
                        ShowColumnHeaderRow = true,
                        HighlightCurrentRow = true,
                        InnerHorizontalBorders = true,
                        InnerVerticalBorders = true,
                        OuterHorizontalBorders = true,
                        OuterVerticalBorders = true,
                        HorizontalBorderPadding = 1,
                        VerticalBorderPadding = 1
                    }
                )
            );
        }

        static void TestExpressions()
        {
            string[] rawSources =
            {
                "  \"Hello world!\"  ",
                "\"Hello \" + \"world!\"",
            };
            foreach (var src in rawSources)
            {
                var engine = new ParseEngine();
                engine.Start(src);
                Console.WriteLine("Source: " + src.ToDisplayString());
                Console.WriteLine("Tokens:");
                foreach (var token in engine.Tokens)
                {
                    Console.WriteLine("    " + token);
                }
                Console.WriteLine();
            }
            //Console.WriteLine("Token types:");
            //foreach (var type in TypeUtil.GetSubTypes(typeof(TokenBase)))
            //    Console.WriteLine("    " + type);

            //var parser = new ParseEngine
            //{
            //    TokenParsed = (token, start, len) => Console.WriteLine("    token at {0}: {1}", start, token)
            //};

            //List<string> inputs = [
            //    "16.4 + 32",
            //    "16.4-32",
            //    "AND(Today() > HighDate, #5/5/1999# <= Today())"
            //];
            //foreach (var input in inputs)
            //{
            //    Console.WriteLine("Parsing: \"" + input + "\"");
            //    parser.Start(input);
            //    Console.WriteLine("    Reconstructed: \"" + parser.ReconstructInput() + "\"");
            //}
            //var expression = "3 + 5 * (2 - 8)";
            //var result = expression.EvaluateExpression();
            //Console.WriteLine($"Expression: {expression}");
            //Console.WriteLine($"Result: {result}");
        }
    }
}
