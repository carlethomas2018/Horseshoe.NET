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
            return HasMatchingTraceGroup(grp => grp.Equals(traceGroup) || grp.StartsWith(traceGroup + "."));
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
        /// Relays a trace message about entering a method including, potentially, any args to registered trace listeners (when tracing is enabled and the specified trace group matches).
        /// </summary>
        /// <param name="methodName">The method name to display. If omitted, system diagnostics and reflection are used to infer the fully qualified method names (including declaring type).</param>
        /// <param name="paramsAndArgs">Optional dictionary of the params and args the developer wishes to relay to the listeners.</param>
        /// <param name="traceGroup">
        /// Trace group identifier (i.e. C# namespace) to match against registered trace groups.
        /// If omitted and calling <see cref="SystemCodeTracing"/><c>.Relay...()</c>, system diagnostics and reflection are used to infer the fully qualified namespace name.
        /// If omitted and calling <see cref="BaseObj"/><c>.Relay...()</c>, <see cref="BaseObj"/><c>.TraceGroup</c> will be used.
        /// </param>
        public static void RelayMethodEntered(string methodName = null, Dictionary<string, object> paramsAndArgs = null, string traceGroup = null)
        {
            if (HasListeners)
            {
                traceGroup ??= new StackTrace().GetFrame(1).GetMethod().DeclaringType.Namespace;

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
        /// Relays a trace message about exiting a method (when tracing is enabled and the specified trace group matches).
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
