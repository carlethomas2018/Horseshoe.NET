using System;
using System.Collections.Generic;
using System.Diagnostics;

using Horseshoe.NET.Collections;
using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.CodeTrace
{
    public static class SystemCodeTracing
    {
        /// <summary>
        /// Collection of relay group names (i.e. C# namespaces) whose code trace messages will be relayed.  Message relaying is opt-in only.
        /// </summary>
        internal static IList<string> TraceGroups { get; set; }

        /// <summary>
        /// Global set of listeners to which code trace messages will be relayed.
        /// </summary>
        internal static IList<ITraceListener> TraceListeners { get; set; }

        public static bool HasTraceGroups => CollectionUtil.HasAny(TraceGroups);

        public static bool HasListeners => CollectionUtil.HasAny(TraceListeners);

        public static bool HasMatchingTraceGroup(string traceGroup)
        {
            return HasMatchingTraceGroup(grp => grp.Equals("*") || grp.Equals(traceGroup) || grp.StartsWith(traceGroup + "."));
        }

        public static bool HasMatchingTraceGroup(Func<string, bool> predicate)
        {
            return CollectionUtil.HasAny(TraceGroups, predicate);
        }

        public static void AddGroups(params string[] traceGroups)
        {
            foreach (string group in traceGroups)
            {
                if (TraceGroups == null)
                    TraceGroups = new List<string>();
                else if (TraceGroups.Contains(group))
                    continue;
                TraceGroups.Add(group);
            }
        }

        public static void RemoveGroups(params string[] traceGroups)
        {
            if (TraceGroups == null)
                return;

            foreach (string group in traceGroups)
            {
                TraceGroups.Remove(group);
            }
        }

        public static void RegisterListener(ITraceListener listener)
        {
            if (TraceListeners == null)
                TraceListeners = new List<ITraceListener>();
            else if (TraceListeners.Contains(listener))
                return;
            TraceListeners.Add(listener);
        }

        public static void UnregisterListener(ITraceListener listener)
        {
            if (TraceListeners == null)
                return;
            TraceListeners.Remove(listener);
        }

        /// <summary>
        /// Relays a trace message to registered trace listeners (when tracing is enabled and the specified trace group matches).
        /// </summary>
        /// <param name="line">Message to relay to the listeners.</param>
        /// <param name="indentAction">Determines how to adjust IIndentable listeners' IndentLevel.</param>
        /// <param name="overrideIndentLevel">Optional indentation level to use for the current message only.</param>
        /// <param name="traceGroup">
        /// Trace group identifier (i.e. C# namespace) to match against registered trace groups.
        /// If omitted and calling <see cref="SystemCodeTracing"/><c>.Relay...()</c>, system diagnostics and reflection are used to infer the fully qualified namespace name.
        /// If omitted and calling <see cref="BaseObj"/><c>.Relay...()</c>, <see cref="BaseObj"/><c>.TraceGroup</c> will be used.
        /// </param>
        public static void RelayMessage(string line, IndentAction indentAction = IndentAction.None, int? overrideIndentLevel = null, string traceGroup = null)
        {
            if (HasListeners)
            {
                traceGroup ??= new StackTrace().GetFrame(1).GetMethod().DeclaringType.Namespace;

                if (HasMatchingTraceGroup(traceGroup))
                {
                    string indentation = string.Empty;
                    foreach (var listener in TraceListeners)
                    {
                        if (listener is IIndentable indentable)
                        {
                            switch (indentAction)
                            {
                                case IndentAction.Increase:
                                    indentable.IndentLevel++;
                                    break;
                                case IndentAction.Decrease:
                                    if (indentable.IndentLevel > 0)
                                        indentable.IndentLevel--;
                                    break;
                                case IndentAction.Reset:
                                    indentable.IndentLevel = 0;
                                    break;
                            }

                            indentation = new string(' ', overrideIndentLevel ?? (indentable.IndentLevel * indentable.IndentWidth));
                        }

                        listener.Relay(line == null ? string.Empty : (indentation + line));

                        if (listener is IIndentable indentableAfter)
                        {
                            switch (indentAction)
                            {
                                case IndentAction.IncreaseAfter:
                                    indentableAfter.IndentLevel++;
                                    break;
                                case IndentAction.DecreaseAfter:
                                    if (indentableAfter.IndentLevel > 0)
                                        indentableAfter.IndentLevel--;
                                    break;
                                case IndentAction.ResetAfter:
                                    indentableAfter.IndentLevel = 0;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Relays a trace multiline message to registered trace listeners (when tracing is enabled and the specified trace group matches).
        /// </summary>
        /// <param name="multiLines">Multiline message to relay to the listeners.</param>
        /// <param name="indentAction">Determines how to adjust IIndentable listeners' IndentLevel.</param>
        /// <param name="overrideIndentLevel">Optional indentation level to use for the current message only.</param>
        /// <param name="traceGroup">
        /// Trace group identifier (i.e. C# namespace) to match against registered trace groups.
        /// If omitted and calling <see cref="SystemCodeTracing"/><c>.Relay...()</c>, system diagnostics and reflection are used to infer the fully qualified namespace name.
        /// If omitted and calling <see cref="BaseObj"/><c>.Relay...()</c>, <see cref="BaseObj"/><c>.TraceGroup</c> will be used.
        /// </param>
        public static void RelayMultiLineMessage(IEnumerable<string> multiLines, IndentAction indentAction = IndentAction.None, int? overrideIndentLevel = null, string traceGroup = null)
        {
            if (HasListeners)
            {
                traceGroup ??= new StackTrace().GetFrame(1).GetMethod().DeclaringType.Namespace;

                if (HasMatchingTraceGroup(traceGroup))
                {
                    string indentation = string.Empty;
                    foreach (var listener in TraceListeners)
                    {
                        if (listener is IIndentable indentable)
                        {
                            switch (indentAction)
                            {
                                case IndentAction.Increase:
                                    indentable.IndentLevel++;
                                    break;
                                case IndentAction.Decrease:
                                    if (indentable.IndentLevel > 0)
                                        indentable.IndentLevel--;
                                    break;
                                case IndentAction.Reset:
                                    indentable.IndentLevel = 0;
                                    break;
                            }

                            indentation = new string(' ', overrideIndentLevel ?? (indentable.IndentLevel * indentable.IndentWidth));
                        }

                        foreach (var line in multiLines)
                        {
                            listener.Relay(line == null ? string.Empty : (indentation + line));
                        }

                        if (listener is IIndentable indentableAfter)
                        {
                            switch (indentAction)
                            {
                                case IndentAction.IncreaseAfter:
                                    indentableAfter.IndentLevel++;
                                    break;
                                case IndentAction.DecreaseAfter:
                                    if (indentableAfter.IndentLevel > 0)
                                        indentableAfter.IndentLevel--;
                                    break;
                                case IndentAction.ResetAfter:
                                    indentableAfter.IndentLevel = 0;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Relays a trace message about an exception that has occurred to registered trace listeners (when tracing is enabled and the specified trace group matches).
        /// </summary>
        /// <param name="exception">The exception to relay to the listeners.</param>
        /// <param name="indentException">Whether to honor IIndentable listeners' IndentLevel, defaut is <c>false</c>.</param>
        /// <param name="throwException">Optionally throw the exception after having relayed it to the listeners, defaut is <c>false</c>.</param>
        /// <param name="traceGroup">
        /// Trace group identifier (i.e. C# namespace) to match against registered trace groups.
        /// If omitted and calling <see cref="SystemCodeTracing"/><c>.Relay...()</c>, system diagnostics and reflection are used to infer the fully qualified namespace name.
        /// If omitted and calling <see cref="BaseObj"/><c>.Relay...()</c>, <see cref="BaseObj"/><c>.TraceGroup</c> will be used.
        /// </param>
        public static void RelayException(Exception exception, bool includeStackTrace = false, bool indentException = false, bool throwException = false, string traceGroup = null)
        {
            if (HasListeners)
            {
                traceGroup ??= new StackTrace().GetFrame(1).GetMethod().DeclaringType.Namespace;

                if (HasMatchingTraceGroup(traceGroup))
                {
                    List<string> lines = new List<string>
                    {
                        null,
                        "* * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
                    };
                    if (exception == null)
                        lines.Add(Lang.Get("Exception.Null"));
                    else if (includeStackTrace)
                        lines.AddRange(exception.RenderWithStackTrace().Split('\r', '\n'));
                    else
                        lines.Add(exception.Render());
                    lines.Add("* * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
                    lines.Add(null);

                    if (indentException)
                        RelayMultiLineMessage(lines, traceGroup: traceGroup);
                    else
                        RelayMultiLineMessage(lines, overrideIndentLevel: 0, traceGroup: traceGroup);
                }
            }

            if (exception != null && throwException)
                throw exception;
        }

        /// <summary>
        /// Relays a trace message about entering a method / constructor and, optionally, its args to registered trace listeners (when tracing is enabled and the specified trace group matches).
        /// </summary>
        /// <param name="methodName">The method name to display. If omitted, system diagnostics and reflection are used to infer the fully qualified method names (including declaring type).</param>
        /// <param name="paramsAndArgs">Optional dictionary of params and args the developer wishes to relay to the listeners.</param>
        /// <param name="param01">Optional first parameter name to relay to the listeners.</param>
        /// <param name="arg01">Optional first argument to relay to the listeners.</param>
        /// <param name="param02">Optional second parameter name to relay to the listeners.</param>
        /// <param name="arg02">Optional second argument to relay to the listeners.</param>
        /// <param name="param03">Optional third parameter name to relay to the listeners.</param>
        /// <param name="arg03">Optional third argument to relay to the listeners.</param>
        /// <param name="param04">Optional fourth parameter name to relay to the listeners.</param>
        /// <param name="arg04">Optional fourth argument to relay to the listeners.</param>
        /// <param name="param05">Optional fifth parameter name to relay to the listeners.</param>
        /// <param name="arg05">Optional fifth argument to relay to the listeners.</param>
        /// <param name="param06">Optional sixth parameter name to relay to the listeners.</param>
        /// <param name="arg06">Optional sixth argument to relay to the listeners.</param>
        /// <param name="param07">Optional seventh parameter name to relay to the listeners.</param>
        /// <param name="arg07">Optional seventh argument to relay to the listeners.</param>
        /// <param name="param08">Optional eighth parameter name to relay to the listeners.</param>
        /// <param name="arg08">Optional eighth argument to relay to the listeners.</param>
        /// <param name="param09">Optional ninth parameter name to relay to the listeners.</param>
        /// <param name="arg09">Optional ninth argument to relay to the listeners.</param>
        /// <param name="param10">Optional tenth parameter name to relay to the listeners.</param>
        /// <param name="arg10">Optional tenth argument to relay to the listeners.</param>
        /// <param name="param11">Optional eleventh parameter name to relay to the listeners.</param>
        /// <param name="arg11">Optional eleventh argument to relay to the listeners.</param>
        /// <param name="param12">Optional twelfth parameter name to relay to the listeners.</param>
        /// <param name="arg12">Optional twelfth argument to relay to the listeners.</param>
        /// <param name="param13">Optional thirteenth parameter name to relay to the listeners.</param>
        /// <param name="arg13">Optional thirteenth argument to relay to the listeners.</param>
        /// <param name="param14">Optional fourteenth parameter name to relay to the listeners.</param>
        /// <param name="arg14">Optional fourteenth argument to relay to the listeners.</param>
        /// <param name="param15">Optional fifteenth parameter name to relay to the listeners.</param>
        /// <param name="arg15">Optional fifteenth argument to relay to the listeners.</param>
        /// <param name="param16">Optional sixteenth parameter name to relay to the listeners.</param>
        /// <param name="arg16">Optional sixteenth argument to relay to the listeners.</param>
        /// <param name="traceGroup">
        /// Trace group identifier (i.e. C# namespace) to match against registered trace groups.
        /// If omitted and calling <see cref="SystemCodeTracing"/><c>.Relay...()</c>, system diagnostics and reflection are used to infer the fully qualified namespace name.
        /// If omitted and calling <see cref="BaseObj"/><c>.Relay...()</c>, <see cref="BaseObj"/><c>.TraceGroup</c> will be used.
        /// </param>
        public static void RelayMethodEntered
        (
            string methodName = null,
            Dictionary<string, object> paramsAndArgs = null,
            string param01 = null, object arg01 = null,
            string param02 = null, object arg02 = null,
            string param03 = null, object arg03 = null,
            string param04 = null, object arg04 = null,
            string param05 = null, object arg05 = null,
            string param06 = null, object arg06 = null,
            string param07 = null, object arg07 = null,
            string param08 = null, object arg08 = null,
            string param09 = null, object arg09 = null,
            string param10 = null, object arg10 = null,
            string param11 = null, object arg11 = null,
            string param12 = null, object arg12 = null,
            string param13 = null, object arg13 = null,
            string param14 = null, object arg14 = null,
            string param15 = null, object arg15 = null,
            string param16 = null, object arg16 = null,
            string traceGroup = null
        )
        {
            if (HasListeners)
            {
                traceGroup ??= new StackTrace().GetFrame(1).GetMethod().DeclaringType.Namespace;
                paramsAndArgs ??= new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(param01)) paramsAndArgs.Add(param01, arg01);
                if (!string.IsNullOrEmpty(param02)) paramsAndArgs.Add(param02, arg02);
                if (!string.IsNullOrEmpty(param03)) paramsAndArgs.Add(param03, arg03);
                if (!string.IsNullOrEmpty(param04)) paramsAndArgs.Add(param04, arg04);
                if (!string.IsNullOrEmpty(param05)) paramsAndArgs.Add(param05, arg05);
                if (!string.IsNullOrEmpty(param06)) paramsAndArgs.Add(param06, arg06);
                if (!string.IsNullOrEmpty(param07)) paramsAndArgs.Add(param07, arg07);
                if (!string.IsNullOrEmpty(param08)) paramsAndArgs.Add(param08, arg08);
                if (!string.IsNullOrEmpty(param09)) paramsAndArgs.Add(param09, arg09);
                if (!string.IsNullOrEmpty(param10)) paramsAndArgs.Add(param10, arg10);
                if (!string.IsNullOrEmpty(param11)) paramsAndArgs.Add(param11, arg11);
                if (!string.IsNullOrEmpty(param12)) paramsAndArgs.Add(param12, arg12);
                if (!string.IsNullOrEmpty(param13)) paramsAndArgs.Add(param13, arg13);
                if (!string.IsNullOrEmpty(param14)) paramsAndArgs.Add(param14, arg14);
                if (!string.IsNullOrEmpty(param15)) paramsAndArgs.Add(param15, arg15);
                if (!string.IsNullOrEmpty(param16)) paramsAndArgs.Add(param16, arg16);

                if (HasMatchingTraceGroup(traceGroup))
                {
                    List<string> lines = new List<string>
                {
                    //Lang.Get("Method.Entering") + ": " +
                    (methodName ?? new StackTrace().GetFrame(1).GetMethod().ToDisplayString()) +   // ref: https://code-maze.com/csharp-how-to-find-caller-method/
                    (CollectionUtil.HasAny(paramsAndArgs) ? string.Empty : "()")
                };

                    if (CollectionUtil.HasAny(paramsAndArgs))
                    {
                        string paramIndentation = new string('\x00A0', 4);  // non-breaking spaces
                        lines.Add("(");
                        foreach (var kvp in paramsAndArgs)
                        {
                            lines.Add(paramIndentation + kvp.Key + ": " + (kvp.Value?.ToDisplayString() ?? "doing"));
                        }
                        lines.Add(")");
                    }

                    RelayMultiLineMessage(lines, traceGroup: traceGroup);

                    RelayMessage("{", indentAction: IndentAction.IncreaseAfter, traceGroup: traceGroup);
                }
            }
        }

        /// <summary>
        /// Relays a trace message about exiting a method / constructor to registered trace listeners (when tracing is enabled and the specified trace group matches).
        /// </summary>
        /// <param name="message">An optional method return related message.</param>
        /// <param name="traceGroup">
        /// Trace group identifier (i.e. C# namespace) to match against registered trace groups.
        /// If omitted and calling <see cref="SystemCodeTracing"/><c>.Relay...()</c>, system diagnostics and reflection are used to infer the fully qualified namespace name.
        /// If omitted and calling <see cref="BaseObj"/><c>.Relay...()</c>, <see cref="BaseObj"/><c>.TraceGroup</c> will be used.
        /// </param>
        public static void RelayMethodReturning(string message = null, string traceGroup = null)
        {
            if (HasListeners)
            {
                traceGroup ??= new StackTrace().GetFrame(1).GetMethod().DeclaringType.Namespace;

                if (HasMatchingTraceGroup(traceGroup))
                {
                    if (message != null)
                        RelayMessage(message, traceGroup: traceGroup);
                    RelayMessage("}", indentAction: IndentAction.Decrease, traceGroup: traceGroup);
                }
            }
        }

        /// <summary>
        /// Relays a trace message about exiting a method including the return value to registered trace listeners (when tracing is enabled and the specified trace group matches).
        /// </summary>
        /// <typeparam name="T">Type of return value</typeparam>
        /// <param name="message">An optional method return related message.</param>
        /// <param name="returnValue">The method return value, including <c>null</c>, to relay to the listeners.</param>
        /// <param name="traceGroup">
        /// Trace group identifier (i.e. C# namespace) to match against registered trace groups.
        /// If omitted and calling <see cref="SystemCodeTracing"/><c>.Relay...()</c>, system diagnostics and reflection are used to infer the fully qualified namespace name.
        /// If omitted and calling <see cref="BaseObj"/><c>.Relay...()</c>, <see cref="BaseObj"/><c>.TraceGroup</c> will be used.
        /// </param>
        public static T RelayMethodReturningValue<T>(string message = null, T returnValue = default, string traceGroup = null)
        {
            if (HasListeners)
            {
                traceGroup ??= new StackTrace().GetFrame(1).GetMethod().DeclaringType.Namespace;

                if (HasMatchingTraceGroup(traceGroup))
                {
                    if (message != null)
                        RelayMessage(message, traceGroup: traceGroup);
                    RelayMessage("returns: " + (returnValue?.ToDisplayString() ?? "doing"), traceGroup: traceGroup);
                    RelayMessage("}", indentAction: IndentAction.Decrease, traceGroup: traceGroup);
                }
            }

            return returnValue;
        }

        private static Languages Lang { get; } = new Languages
        {
            { "Exception.Null", "Null exception" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Exception.Null", "El error es nulo" },
            }
        );
    }
}
